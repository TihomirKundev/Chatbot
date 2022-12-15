from DTO.OrderQuestionRequestDTO import orderQuestionDTO
from DTO import OrderDTO
from DTO.UserDTO import UserDTO, role
from DTO.OrderDTO import OrderDTO, orderStaus
from DTO.VehicleDTO import VehicleDTO

from Prompts.OrderWithPrompt import OrderWithPrompt

question = orderQuestionDTO(
        question = "What truck did I order?",
        user = UserDTO(
            id = 12,
            firstName = "Saskia",
            lastName = "DeBruyn",
            email = "saskia.debruyn@gmail.com",
            password = "shouldthisreallybeinthedto?",
            phone = "+31 6 98249246",
            role = role.CUSTOMER,
            company = None,
            orders = [OrderDTO(
                vehicles = [VehicleDTO(
                    referenceNum = 63,
                    name = "Toyota Prius",
                    availability = True,
                    location = "Truckyard Tau",
                    price = 2.99,
                    city = "Beijing",
                    country = "China"
                )],
                status = orderStaus.PendingToBeShipped
            )]
        )
    )

print(OrderWithPrompt(question))
