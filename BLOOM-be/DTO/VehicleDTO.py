from pydantic import BaseModel


class VehicleDTO(BaseModel):
    referenceNum: int
    name: str
    availability: bool
    location: str
    price: float
    city: str
    country: object
