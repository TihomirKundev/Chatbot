from pydantic import BaseModel
from enum import Enum

from DTO.OrderDTO import OrderDTO


class role(Enum):
    CUSTOMER = 0
    ADMIN = 1


class UserDTO(BaseModel):
    id: str
    firstName: str
    lastName: str
    email: str
    password: str
    phone: str
    role: role
    company: object  # will fill up later
    orders: list[OrderDTO]
