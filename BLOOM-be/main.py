from fastapi import FastAPI
from pydantic import BaseModel

import questionAnswerer
from DTO.OrderQuestionRequestDTO import orderQuestionDTO
from DTO.UserDTO import UserDTO
from DTO.faqQuestionRequestDTO import faqQuestionDTO


app = FastAPI()

@app.get("/orderAnswer/")
async def orderAnswer(incOrderQuestion: orderQuestionDTO):
    return questionAnswerer.answerOrderQuestion(incOrderQuestion)


@app.get("/faqAnswer/")
async def faqAnswer(incFaqQuestion: faqQuestionDTO):
    return questionAnswerer.answerFAQQuestion(incFaqQuestion.question)

