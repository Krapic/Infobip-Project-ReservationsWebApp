import React, { useState, useEffect } from 'react';
import { Route, Routes } from 'react-router-dom';
import { ColorModeContext, useMode } from './theme.js';
import { CssBaseline, ThemeProvider } from '@mui/material';
import TopBar from './scenes/global/TopBar.jsx';
import Footer from './scenes/global/Footer.jsx';
import Main from './scenes/RezervacijeSchedule/Main.jsx'
import Login from './scenes/login/login.jsx';
import UserProfile from './scenes/user/UserProfile.jsx';
import RegisterNewBusiness from './scenes/registernewbusiness/registernewbusiness.jsx';
import './index.css';
import { UserContext } from './scenes/login/UserContext.js';
import AdminAuthentifications from './scenes/adminWorkplace/AdminAuthentifications.jsx';
import Cookies from 'js-cookie';

function App(){
  const userEmail = Cookies.get('email');
  const [theme, colorMode] = useMode();
  const [email, setEmail] = useState('');
  const [businessCount, setBusinessCount] = useState(10);
  const [queryString, setQueryString] = useState(userEmail ?
    `?email=${encodeURIComponent(userEmail)}&businessCount=${encodeURIComponent(businessCount)}` : 
    `?businessCount=${encodeURIComponent(businessCount)}`);
  const [dataArray, setDataArray] = useState([]);
  const [notFound, setNotFound] = useState(false);
  const [responseMessage, setResponseMessage] = useState("");
  const [isReadonly, setIsReadonly] = useState(false);
  const [actualBusinessCount, setActualBusinessCount] = useState(10);
  
  const fetchData = async (query) => {
    try {
        if(userEmail === undefined){
          setIsReadonly(true);
        }

        const response = await fetch(`${process.env.REACT_APP_BACKEND_URL}/api/business/list${query}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const data = await response.json();

        if (!response.ok) {
          setNotFound(true);
          setResponseMessage(data.response);
          return;
        }

        setNotFound(false);
        setDataArray(data.listOfBusinesses.$values);
        setActualBusinessCount(data.businessCount);
        
    } catch (error) {
    }
  };

  useEffect(() => {
    fetchData(queryString);
  }, []);

  const readMoreBusinesses = () => {
    setBusinessCount(businessCount + 10);
    fetchData();
  };

  return (
    <UserContext.Provider value={email}>
    <ColorModeContext.Provider value={colorMode}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <div className='app'>
          <main className='content'>
            <TopBar setQueryString={setQueryString} queryString={queryString} fetchData={fetchData} />
            <Routes>
              <Route exact path="/" element=
              {<Main
                dataArray={dataArray}
                notFound={notFound}
                responseMessage={responseMessage}
                isReadonly={isReadonly}
                actualBusinessCount={actualBusinessCount}
                readMoreBusinesses={readMoreBusinesses}

              />} />
              <Route exact path="/login" element={<Login setEmailGlobal={setEmail} />} />
              <Route exact path="/user/:activepage" element={<UserProfile />} />
              <Route exact path="/registernewbusiness" element={<RegisterNewBusiness />} />
              <Route exact path="/admin" element={<AdminAuthentifications />} />
            </Routes>
            <Footer />
          </main>
        </div>
      </ThemeProvider>
    </ColorModeContext.Provider>
    </UserContext.Provider>
  );
}

export default App;
