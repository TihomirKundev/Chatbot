from pydantic import BaseModel

from DTO.UserDTO import UserDTO


class orderQuestionDTO(BaseModel):
    question: str
    user: UserDTO
