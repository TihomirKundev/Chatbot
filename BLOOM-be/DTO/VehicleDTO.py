from pydantic import BaseModel


class VehicleDTO(BaseModel):
    ReferenceNum: int
    Name: str
    Availability: bool
    Location: str
    Price: float
    City: str
    Country: object
