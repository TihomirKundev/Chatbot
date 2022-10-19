﻿from transformers import AutoModelForCausalLM, AutoTokenizer, set_seed, StoppingCriteria
import torch

import FAQwirhPromt

# torch.set_default_tensor_type(torch.cuda.FloatTensor)
# torch.cuda.empty_cache()
model = AutoModelForCausalLM.from_pretrained("bigscience/bloom-1b1", use_cache=True)
tokenizer = AutoTokenizer.from_pretrained("bigscience/bloom-1b1")


# set_seed(42)

def answerQuestion(UserQuestion):
    prompt = FAQwirhPromt.faqPromt + UserQuestion
    input_ids = tokenizer(prompt, return_tensors="pt") # .to(0)
    sample = model.generate(**input_ids, max_length=1255, top_k=0, temperature=1.7)
    result: str = tokenizer.decode(sample[0], truncate_before_pattern=[r"\n\n^#", "^'''", "\n\n\n"])
    slicedRes = result[1205 + len(UserQuestion):len(result)]  # get only the answer
    if("A25:" in slicedRes):
        slicedRes = slicedRes[slicedRes.find("A25")+4:] # remove the A25: from the beginning of the answer
    if("Q26" in slicedRes):
        slicedRes = slicedRes[0:slicedRes.find("Q26")]
    if("." in slicedRes):
        slicedRes = slicedRes[0:slicedRes.find(". ")]
    return slicedRes