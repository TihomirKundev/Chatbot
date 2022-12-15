from transformers import AutoModelForCausalLM, AutoTokenizer

from Prompts import FAQwirhPromt, ModelPrompt
from DTO.OrderQuestionRequestDTO import orderQuestionDTO
from Prompts.OrderWithPrompt import OrderWithPrompt
import torch

torch.set_default_tensor_type(torch.cuda.FloatTensor)
torch.cuda.empty_cache()
model = AutoModelForCausalLM.from_pretrained("bigscience/bloom-1b1", use_cache=True)
tokenizer = AutoTokenizer.from_pretrained("bigscience/bloom-1b1")


# set_seed(42)

def answerFAQQuestion(UserQuestion):
    prompt = FAQwirhPromt.getPromptWithQuestion(UserQuestion)
    input_ids = tokenizer(prompt, return_tensors="pt").to('cuda:0')
    sample = model.generate(**input_ids, max_new_tokens=40)
    result: str = tokenizer.decode(sample[0][input_ids.input_ids.shape[1]:])
    if "\n" in result:
        result = result[1:result.find("\n")]
    return result

def answerOrderQuestion(UserQuestion: orderQuestionDTO):
    prompt = OrderWithPrompt(UserQuestion)
    input_ids = tokenizer(prompt, return_tensors="pt").to('cuda:0')
    sample = model.generate(**input_ids, max_new_tokens=40)
    result: str = tokenizer.decode(sample[0][input_ids.input_ids.shape[1]:])
    if "\n" in result:
        result = result[1:result.find("\n")]
    return result

def determineModel(userQuestion):
    order_token = 46478
    faq_token = 209147

    prompt = ModelPrompt.getPromptWithQuestion(userQuestion)
    input_ids = tokenizer(prompt, return_tensors="pt").to('cuda:0')
    sample = model.generate(**input_ids, max_new_tokens=1, do_sample=False, output_scores=True, return_dict_in_generate=True)

    scores = sample.scores[0].softmax(1)[0]
    order_probability = scores[order_token]
    faq_probability = scores[faq_token]

    probability_of_order_over_faq = order_probability - faq_probability
    print('probdiff: ' + str(probability_of_order_over_faq))

    return "order" if probability_of_order_over_faq > 0.7 else "faq"
