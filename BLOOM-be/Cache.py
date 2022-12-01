from transformers import AutoModelForCausalLM, AutoTokenizer
import os

MODEL_CACHE = ".cache/model.cache"
TOKENIZER_CACHE = ".cache/tokenizer.cache"

def getModels(store_cache=False):
    if(os.path.exists(MODEL_CACHE)):
        model = AutoModelForCausalLM.from_pretrained(MODEL_CACHE)
    else:
        model = AutoModelForCausalLM.from_pretrained("bigscience/bloom-1b1", use_cache=True)
        if(store_cache):
            model.save_pretrained(MODEL_CACHE)
    
    if(os.path.exists(TOKENIZER_CACHE)):
        tokenizer = AutoTokenizer.from_pretrained(TOKENIZER_CACHE)
    else:
        tokenizer = AutoTokenizer.from_pretrained("bigscience/bloom-1b1", use_cache=True)
        if(store_cache):
            tokenizer.save_pretrained(TOKENIZER_CACHE)
    
    return model, tokenizer

def downloadModels(store_cache=True):
    model = AutoModelForCausalLM.from_pretrained("bigscience/bloom-1b1", use_cache=True)
    tokenizer = AutoTokenizer.from_pretrained("bigscience/bloom-1b1", use_cache=True)
    if(store_cache):
        model.save_pretrained(MODEL_CACHE)
        tokenizer.save_pretrained(TOKENIZER_CACHE)
    return model, tokenizer
        