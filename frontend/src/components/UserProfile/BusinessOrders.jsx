import { registerLicense } from '@syncfusion/ej2-base';
import { ScheduleComponent, Week, Inject, ViewsDirective, ViewDirective } from '@syncfusion/ej2-react-schedule';
import { useTheme } from "@mui/material";
import { useState, useEffect } from "react";
import Cookies from 'js-cookie';
import ErrorComponent from '../../scenes/global/ErrorComponent'

registerLicense('Ngo9BigBOggjHTQxAR8/V1NAaF5cWWRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWX5cdXRTRWJYVUF2V0s=');

const BusinessOrders = () => {
    const userEmail = Cookies.get('email');
    const jwt = Cookies.get('jwt');
    const theme = useTheme();
    const [scheduleData, setScheduleData] = useState({});
    const [errorMessage, setErrorMessage] = useState('');

    const fetchData = async () => {
        let queryString = '';
        if(userEmail !== undefined){
            queryString = `?UserEmail=${encodeURIComponent(userEmail)}`;
        }

        try {
            const response = await fetch(`${process.env.REACT_APP_BACKEND_URL}/api/reservation/appointments-for-business${queryString}`, {
                method: 'GET',
                headers: {
                    'Authorization' : `Bearer ${jwt}`,
                    'Content-Type': 'application/json'
                }
            });
    
            const data = await response.json();
            console.log(data);
    
            if (!response.ok) {
                setErrorMessage(data.response);
                return;
            }

            setScheduleData(data.appointments.$values);
            
        } catch (error) {
        }
      };

    useEffect(() => {
        fetchData();
    }, []);

    const eventSettings = { dataSource: scheduleData, enableMaxHeight: true };

    return (
        <>
            <div style={{ height: '100%', overflowY: 'auto' }}>
            {theme.palette.mode === 'dark' ? 
                <link href="https://cdn.syncfusion.com/ej2/22.1.34/bootstrap5-dark.css" rel="stylesheet"/>
                :
                <link href="https://cdn.syncfusion.com/ej2/22.1.34/bootstrap5.css" rel="stylesheet"/>
            }
            
            <ScheduleComponent
                eventSettings={eventSettings}
                readonly={true}
                showWeekend={false}
                allowDragAndDrop={false}
                allowResizing={false}
                allowInline={false}
                >
                <ViewsDirective>
                    <ViewDirective option='Week' />
                </ViewsDirective>
                <Inject services={[Week]}/>
            </ScheduleComponent>
            </div>
            <div style={{ marginTop: '-1px' }}>
                <ErrorComponent errorMessage={errorMessage} />
            </div>
        </>
    );
}

export default BusinessOrders;