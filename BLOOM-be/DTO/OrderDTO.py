from enum import Enum

from pydantic import BaseModel

from DTO.VehicleDTO import VehicleDTO

class orderStaus(Enum):
    PendingToBeShipped = 0
    Delivered = 1
    Canceled = 2
    Shipped = 3
    ProblemWithDelivery = 4


class OrderDTO(BaseModel):
    Id: str
    Vehicles: list[VehicleDTO]
    Status: orderStaus

