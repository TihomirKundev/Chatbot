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

    def to_json(self):
        return {
            "First Name": self.firstName,
            "Last Name": self.lastName,
            "E-Mail": self.email,
            "Phone": self.phone,
            "Orders": [order.to_json() for order in self.orders]
        }
