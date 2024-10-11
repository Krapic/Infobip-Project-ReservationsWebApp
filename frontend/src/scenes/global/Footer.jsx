import './Footer.css';
import { useTheme } from "@mui/material";

const Footer = () => {
    const theme = useTheme();

    return (
        <div className="bottom-div" style={{ 
            backgroundColor: theme.palette.mode === 'dark' ? '#2F2F2F' : '#FCFCFC' }}>
            <img 
                src={theme.palette.mode === 'dark' ? "/logo/Logo150DM.png" : "/logo/Logo150LM.png"}
                alt="Logo"
                style={{ width: '25px', height: '25px', marginRight: '10%' }} />
            <div className='rezervacije-contact-info'>
                Admin support: rezervacije@gmail.com
            </div>
        </div>
    );
}

export default Footer;