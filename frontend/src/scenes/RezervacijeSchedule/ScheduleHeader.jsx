import './ScheduleHeader.css';
import { useTheme } from "@mui/material";

const ScheduleHeader = (props) => {
    const theme = useTheme();

    return (
      <>
        <div style={{ display: 'flex', marginLeft: '5%', marginRight: '5%', marginBottom: '1%', paddingTop: '1%', alignItems: 'center', maxHeight: '25%' }} >
            <div className='header-title'>
              <p>Kontakt:</p>
              <p style={{marginTop: '-1%'}}>{props.phoneNumber}</p>
            </div>
            
            <div className='header-business-title'>
              {props.businessName}
            </div>

            <div className='header-title'>
              <p>Adresa:</p>
              <p style={{marginTop: '-1%'}}>{props.address}</p>
            </div>
        </div>
      </>
    );
  };
  
  export default ScheduleHeader;