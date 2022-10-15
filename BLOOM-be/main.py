from fastapi import FastAPI
from pydantic import BaseModel

import questionAnswerer
from DTO.faqQuestionRequestDTO import faqQuestionDTO

app = FastAPI()


@app.get("/")
async def root():
    return {"message": "Hello World"}


# @app.get("/hello/{name}")
# async def say_hello(name: str):
#     return {"message": f"Hello {name}"}


@app.get("/faqAnswer/")
async def faqAnswer(incFaqQuestion: faqQuestionDTO):
    return questionAnswerer.answerQuestion(incFaqQuestion.question)