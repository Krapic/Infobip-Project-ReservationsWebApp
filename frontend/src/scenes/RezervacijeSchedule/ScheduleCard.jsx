import './ScheduleCard.css';
import ScheduleHeader from './ScheduleHeader';
import ScheduleBody from './ScheduleBody';
import { useTheme } from "@mui/material";


const ScheduleCard = (props) => {
    const theme = useTheme();
    return (
      <>
        <div style={{ backgroundColor: theme.palette.mode === 'dark' ? "#2F2F2F" : "#fafafa", marginTop: '10%' }} className="schedule-background">
            <ScheduleHeader 
              phoneNumber={`${props.data.DialingCode} ${props.data.PhoneNumber}`} 
              businessName={props.data.BusinessName}
              address={
                `${props.data.Address.Street} 
                 ${props.data.Address.HouseNumber},
                 ${props.data.Address.PostalCode} 
                 ${props.data.Address.Town}
                `
              }
            />

            <ScheduleBody 
              businessActivities = {props.data.BusinessActivities}
              workHours = {props.data.WorkHours}
              appointments = {props.data.Appointments}
              businessName = {props.data.BusinessName}
              minimumHour = {props.data.MinimumHour}
              maximumHour = {props.data.MaximumHour}
              isReadonly = {props.isReadonly}
            />
        </div>
      </>
    );
  };
  
  export default ScheduleCard;