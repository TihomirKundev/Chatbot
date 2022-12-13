from transformers import AutoModelForCausalLM, AutoTokenizer

from Prompts import FAQwirhPromt, ModelPrompt
from DTO.OrderQuestionRequestDTO import orderQuestionDTO
from Prompts.OrderWithPrompt import OrderWithPrompt

# torch.set_default_tensor_type(torch.cuda.FloatTensor)
# torch.cuda.empty_cache()
model = AutoModelForCausalLM.from_pretrained("bigscience/bloom-1b1", use_cache=True)
tokenizer = AutoTokenizer.from_pretrained("bigscience/bloom-1b1")


# set_seed(42)

def answerFAQQuestion(UserQuestion):
    prompt = FAQwirhPromt.faqPromt + UserQuestion
    input_ids = tokenizer(prompt, return_tensors="pt") # .to(0)
    sample = model.generate(**input_ids, max_length=1350, top_k=0, temperature=1.7)
    result: str = tokenizer.decode(sample[0], truncate_before_pattern=[r"\n\n^#", "^'''", "\n\n\n"])
    slicedRes = result[1205 + len(UserQuestion):len(result)]  # get only the answer
    if("A27:" in slicedRes):
        slicedRes = slicedRes[slicedRes.find("A27")+4:] # remove the A25: from the beginning of the answer
    if("Q28" in slicedRes):
        slicedRes = slicedRes[0:slicedRes.find("Q28")]
    if("." in slicedRes):
        slicedRes = slicedRes[0:slicedRes.find(". ")]
    return slicedRes

def answerOrderQuestion(UserQuestion: orderQuestionDTO):
    prompt = OrderWithPrompt(UserQuestion)
    input_ids = tokenizer(prompt, return_tensors="pt") # .to(0)
    sample = model.generate(**input_ids, max_length=170, top_k=0, temperature=1.7)
    result: str = tokenizer.decode(sample[0], truncate_before_pattern=[r"\n\n^#", "^'''", "\n\n\n"])
    slicedRes = result[result.find("Answer:")+7:len(result)]  # get only the answer
    if("  \nA:" in slicedRes):
        slicedRes = slicedRes[slicedRes.find("  \nA:")+6:] # remove the A: from the beginning of the answer    
        if("\nB:" in slicedRes):
            slicedRes = slicedRes[0:slicedRes.find("\nB:")]
        if("\nA:" in slicedRes):
            slicedRes = slicedRes[0:slicedRes.find("\nA:")]
    else:
        if("A:" in slicedRes):
            slicedRes = slicedRes[0:slicedRes.find("A:")] # remove the A25: from the beginning of the answer
        if("." in slicedRes):
            slicedRes = slicedRes[0:slicedRes.find(". ")]
    return slicedRes

def determineModel(userQuestion):
    order_token = 46478
    faq_token = 209147

    prompt = ModelPrompt.getPromptWithQuestion(userQuestion)
    input_ids = tokenizer(prompt, return_tensors="pt") # .to(0)
    sample = model.generate(**input_ids, max_new_tokens=1, do_sample=False, output_scores=True, return_dict_in_generate=True)

    scores = sample.scores[0].softmax(1)[0]
    order_probability = scores[order_token]
    faq_probability = scores[faq_token]

    probability_of_order_over_faq = order_probability - faq_probability

    return "order" if probability_of_order_over_faq > 0.5 else "faq"
