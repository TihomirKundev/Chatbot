from transformers import AutoModelForCausalLM, AutoTokenizer

from Prompts import FAQwirhPromt
from DTO.OrderQuestionRequestDTO import orderQuestionDTO
from Prompts.OrderWithPrompt import OrderWithPrompt

import Cache


import nltk
import sklearn

def FAQanswer(question):
  prompt_dict = FAQwirhPromt.prompt

  # Use NLTK's word_tokenize method to split the question into individual words
  tokenized_question = nltk.word_tokenize(question)

  # Create an empty list to store the tokenized questions from the prompt dictionary
  tokenized_prompts = []

  # Iterate through the prompt dictionary and tokenize the questions
  for key, value in prompt_dict.items():
    tokenized_prompts.append(nltk.word_tokenize(key))

  # Use scikit-learn's CountVectorizer to create a bag-of-words representation of the tokenized text
  vectorizer = sklearn.feature_extraction.text.CountVectorizer()
  question_vectors = vectorizer.fit_transform(tokenized_question)
  prompt_vectors = vectorizer.transform(tokenized_prompts)

  # Use scikit-learn's cosine similarity method to find the similarity between the question and the prompt dictionary
  similarity = sklearn.metrics.pairwise.cosine_similarity(question_vectors, prompt_vectors)

  # Find the index of the most similar question in the prompt dictionary to the question
  most_similar_question_index = similarity.argmax()

  # Return the answer from the prompt dictionary that corresponds to the most similar question
  return prompt_dict[tokenized_prompts[most_similar_question_index]]





# torch.set_default_tensor_type(torch.cuda.FloatTensor)
# torch.cuda.empty_cache()

# model = AutoModelForCausalLM.from_pretrained("bigscience/bloom-1b1", use_cache=True)
# tokenizer = AutoTokenizer.from_pretrained("bigscience/bloom-1b1")

model, tokenizer = Cache.getModels();

# set_seed(42)

def answerFAQQuestion(UserQuestion):
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