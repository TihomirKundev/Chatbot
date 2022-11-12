from pydantic import BaseModel
from enum import Enum

from DTO.OrderDTO import OrderDTO


class role(Enum):
    CUSTOMER = 0
    ADMIN = 1


class UserDTO(BaseModel):
    ID: str
    FirstName: str
    LastName: str
    Email: str
    Password: str
    Phone: str
    Role: role
    Company: object  # will fill up later
    Orders: list[OrderDTO]
