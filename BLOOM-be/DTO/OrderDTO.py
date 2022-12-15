from enum import Enum

from pydantic import BaseModel

from DTO.VehicleDTO import VehicleDTO

class orderStaus(Enum):
    PendingToBeShipped = 0
    Delivered = 1
    Canceled = 2
    Shipped = 3
    ProblemWithDelivery = 4

    def to_str(self):
        if self == orderStaus.PendingToBeShipped:
            return "Pending"
        elif self == orderStaus.ProblemWithDelivery:
            return "There was a problem with delivery"
        else:
            return str(self)


class OrderDTO(BaseModel):
   #TODO:add orderID
   vehicles: list[VehicleDTO]
   status: orderStaus

   def to_json(self):
       return {
           "Status": self.status.to_str(),
           "Vehicles": [vehicle.to_json() for vehicle in self.vehicles]
       }

