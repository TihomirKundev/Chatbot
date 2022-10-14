import axios from "axios";
import {BaseUrl, LoginApi} from "../../app.properties";

const login = async (request) => {
    return await axios.post(BaseUrl + LoginApi, request);
}

export default {
    login
}