import { useTheme } from "@mui/material";
import './DataCard.css';

const DataCard = (props) => {
    const theme = useTheme();

    return (
        <>
            <div style={{ width: '100%' }}>
                <p className='admin-headers' >{props.header}</p>
                <div className='admin-container' style={{
                background: theme.palette.mode !== 'dark' 
                    ? 'linear-gradient(to bottom, rgba(200, 200, 200, 0.3), rgba(200, 200, 200, 0.2))' 
                    : 'linear-gradient(to bottom, rgba(0, 0, 0, 0.3), rgba(0, 0, 0, 0.2))',
                color: theme.palette.mode !== 'dark'
                    ? '#757575'
                    : 'white'
            }} >
                {Array.isArray(props.data) ? (
                    props.data.map((x, index) => <p key={index} className='admin-paragraphs'>{x}</p>)
                ) : (
                    <p className='admin-paragraphs'>{props.data}</p>
                )}
                </div>
            </div>
        </>
    );
};
  
export default DataCard;