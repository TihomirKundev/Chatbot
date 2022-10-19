import axios from "axios";
import {BaseUrl, RegisterApi} from "../../app.properties";


const register = async (data) => {
    return axios.post(BaseUrl + RegisterApi, data);
}

export default {
    register
}