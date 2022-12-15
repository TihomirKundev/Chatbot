from fastapi import FastAPI
from pydantic import BaseModel

import questionAnswerer
from DTO.OrderQuestionRequestDTO import orderQuestionDTO
from DTO.UserDTO import UserDTO
from DTO.faqQuestionRequestDTO import faqQuestionDTO
from fastapi.responses import PlainTextResponse

app = FastAPI()



@app.get("/orderAnswer/")
async def orderAnswer(incOrderQuestion: orderQuestionDTO):
    return questionAnswerer.answerOrderQuestion(incOrderQuestion)


@app.get("/faqAnswer/", response_class=PlainTextResponse)
async def faqAnswer(incFaqQuestion: faqQuestionDTO):
    return questionAnswerer.answerFAQQuestion(incFaqQuestion.question)

@app.post("/modelClassification/", response_class=PlainTextResponse)
async def modelClassification(incFaqQuestion: faqQuestionDTO):
    return questionAnswerer.determineModel(incFaqQuestion.question)
