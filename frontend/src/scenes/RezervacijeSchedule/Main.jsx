import { registerLicense } from '@syncfusion/ej2-base';
import ScheduleCard from './ScheduleCard';
import ErrorComponent from '../global/ErrorComponent';
import { useTheme } from "@mui/material";

import './Main.css';

registerLicense('Ngo9BigBOggjHTQxAR8/V1NAaF5cWWRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWX5cdXRTRWJYVUF2V0s=');

const Main = ({ dataArray, notFound, responseMessage, isReadonly, actualBusinessCount, readMoreBusinesses }) => {
  const theme = useTheme();

  if(notFound){
    return (
      <ErrorComponent errorMessage={responseMessage} />
    );
  }

  return (
    <>
      {dataArray.map((d, index) => (
        <ScheduleCard key={index} data={d} isReadonly={isReadonly} />
      ))}

      <div style={{ margin: '1%', display:'flex', justifyContent: 'center', alignItems: 'center' }}>
        <button
          style={{ 
            color: theme.palette.mode === 'dark' ? '#141B2D' : 'white',
            backgroundColor: '#70D8BD',
            ...(actualBusinessCount % 10 !== 0 && { backgroundColor: '#CCCCCC' })
          }}
          className='read-more-businesses'
          onClick={readMoreBusinesses}
          disabled={actualBusinessCount % 10 !== 0}
        >
        Učitaj
        </button>
      </div>
    </>
  );
};
  
export default Main;