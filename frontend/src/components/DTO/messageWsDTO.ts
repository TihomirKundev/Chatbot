export type MessageWsDTO = {
    Action: string,
    Content: string,
    Nickname: string,
    Timestamp: string,
    QuickSelector: quickSelector
}
export enum quickSelector {
    faq = 0  ,
    ts = 1 ,
    order = 2 
}