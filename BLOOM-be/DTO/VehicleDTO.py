from pydantic import BaseModel


class VehicleDTO(BaseModel):
    referenceNum: int
    name: str
    availability: bool
    location: str
    price: float
    city: str
    country: object

    def to_json(self):
        return {
            "Vehicle": self.referenceNum,
            "Name": self.name,
            "Location": self.location + ", " + self.city + ", " + self.country,
            "Price": f"€ {self.price}"
        }
