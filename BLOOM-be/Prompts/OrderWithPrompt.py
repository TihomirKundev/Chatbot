from DTO.OrderQuestionRequestDTO import orderQuestionDTO


def OrderWithPrompt(incOrderQuestion: orderQuestionDTO):
    Prompt = f'''
Customer:
customerId:{incOrderQuestion.user.ID}
firstName:{incOrderQuestion.user.FirstName}
lastName:{incOrderQuestion.user.LastName}
email:{incOrderQuestion.user.Email}
phone:{incOrderQuestion.user.Phone}
    '''
    for o in incOrderQuestion.user.Orders:
        Prompt += f'''
Order:
orderNumber:{o.Id}
orderStatus:{o.Status.name}
        '''
        for v in o.Vehicles:  # TODO:maybe change Vehicle to truck
            Prompt += f'''
Vehicle:
VehicleReferenceNum:{v.ReferenceNum}
Name:{v.Name}
availability:{v.Availability}
location:{v.Location}
price:{v.Price}
city:{v.City}
            '''
    Prompt += f'''
Question:{incOrderQuestion.question}
Answer:
    '''
    return Prompt
