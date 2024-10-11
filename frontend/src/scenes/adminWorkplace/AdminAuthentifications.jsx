import { useEffect, useState } from 'react';
import { useTheme } from "@mui/material";
import DataCard from './DataCard';
import Cookies from 'js-cookie';
import Checkbox from '@mui/material/Checkbox';
import FormControlLabel from '@mui/material/FormControlLabel';
import ErrorComponent from '../global/ErrorComponent'
import './AdminAuthentifications.css';

const AdminAuthentifications = () => {
  const theme = useTheme();
  const jwt = Cookies.get('jwt'); 
  const [data, setData] = useState({});
  const [isError, setIsError] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [adminComment, setAdminComment] = useState('');
  const [isValidBusiness, setIsValidBusiness] = useState(false);

  useEffect(() => {

    const fetchData = async () => {
      try {
        const response = await fetch(`${process.env.REACT_APP_BACKEND_URL}/api/business/auth/list`, {
            method: 'GET',
            headers: {
              'Authorization' : `Bearer ${jwt}`,
              'Content-Type': 'application/json'
            }
        });

        const temp = await response.json();
        

        if (!response.ok) {
          throw new Error(temp.response);
        }

        setData(temp.business);
        console.log(temp.business);
          
      } catch (error) {
        setErrorMessage(error.message);
        setIsError(true);
      }
    };

    fetchData();
  }, []);

  const handleTextAreaChange = (event) => {
    setAdminComment(event.target.value);
  };

  const handleCheckboxChange = (event) => {
    setIsValidBusiness(event.target.checked);
  };

  const submitWork = () => {
    const fetchData = async () => {
      try {
        const response = await fetch(`${process.env.REACT_APP_BACKEND_URL}/api/business/auth`, {
            method: 'POST',
            headers: {
              'Authorization' : `Bearer ${jwt}`,
              'Content-Type': 'application/json'
            },
            body: JSON.stringify({
              IsValidBusiness: isValidBusiness,
              AdminComment: adminComment,
              BusinessName: data.BusinessName
            })
        });

        const temp = await response.json();
        

        if (!response.ok) {
          return;
        }
          
      } catch (error) {
      }
    };

    fetchData();

    setTimeout(() => {
      window.location.reload();
    }, 2000);
  };

  if(isError){
    return(
      <ErrorComponent errorMessage={errorMessage} />
    );
  }

  return (
      <>
        {data.User !== undefined &&
          <>
            <div className='row-class'>
              <DataCard header='Email usera:' data={data.User.Email} />
              <DataCard header='Telefonski broj usera:' data={data.User.PhoneNumber} />
            </div>

            <div className='row-class'>
              <DataCard header='Grad:' data={data.Address.Town} />
              <DataCard header='Država:' data={data.Address.Country} />
              <DataCard header='Ulica i kućni broj:' data={`${data.Address.Street} ${data.Address.HouseNumber}`} />
              <DataCard header='Poštanski broj:' data={data.Address.PostalCode} />
            </div>

            <div className='row-class'>
              <DataCard header='Ime obrta:' data={data.BusinessName} />
              <DataCard header='Identifikacijski broj obrta:' data={data.BusinessIdentificationNumber} />
              <DataCard header='Vrsta obrta:' data={data.BusinessType} />
            </div>

            <div className='row-class'>
              <DataCard header='Usluge:' 
                data={data.BusinessActivities.$values.map(activity => 
                `${activity.NameOfActivity} - 
                 ${activity.DescriptionOfActivity} - 
                 ${activity.Price}€`)}
              />

              <DataCard header='Radni dani:' 
                data={data.WorkingDayStructures.$values.map(activity =>
                `Day ${activity.Day + 1}: 
                 ${activity.StartingHours} - 
                 ${activity.EndingHours}`
                )} 
              />
            </div>

            <div className='bottom-content'>
              <textarea 
                  style={{ 
                    backgroundColor: 'transparent', 
                    border: theme.palette.mode === 'dark' ? '1px solid' : 'none',
                    borderColor: theme.palette.mode === 'dark' ? '#2F2F2F' : 'transparent'
                  }} 
                  className="admin-comment" 
                  placeholder='Ukratko objasnite svoju odluku: ' 
                  rows="4" 
                  cols="50"
                  value={adminComment}
                  onChange={handleTextAreaChange}
              ></textarea>

              <FormControlLabel
                control={<Checkbox 
                  checked={isValidBusiness} 
                  onChange={handleCheckboxChange}
                  size="large"
                  style={{ color: theme.palette.mode === 'dark' ? 'white' : '#888' }}
                  />}

                label="Dozvola za oglašavanje na stranici"
                sx={{ '& .MuiTypography-root': { fontSize: '16px' }, margin: '1%' }} 
              />

              <button 
                  className='confirm-button'
                  style={{ backgroundColor: theme.palette.mode === 'dark' ? '#303030' : '#888' }}
                  onClick={submitWork}
              >Potvrdi
              </button>
            </div>
          </>
        }
      </>
  );
};
  
export default AdminAuthentifications;