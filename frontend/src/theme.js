import {createTheme} from "@mui/material/styles";

export const theme = createTheme({
        palette: {
            type: 'light',
            primary: {
                main: '#05ad61',
                contrastText: '#060606',
            },
            secondary: {
                main: '#000000',
                contrastText: '#ece2e2',
            },
            background: {
                default: '#ffffff',
            },
        }}
)