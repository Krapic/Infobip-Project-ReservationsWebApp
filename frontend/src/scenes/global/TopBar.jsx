import { Box, IconButton, InputBase, useTheme } from "@mui/material";
import { useContext, useState, useEffect } from "react";
import { ColorModeContext, tokens } from "../../theme";
import LightModeOutlinedIcon from "@mui/icons-material/LightModeOutlined";
import DarkModeOutlinedIcon from "@mui/icons-material/DarkModeOutlined";
//import NotificationsOutlinedIcon from "@mui/icons-material/NotificationsOutlined";
//import SettingsOutlinedIcon from "@mui/icons-material/SettingsOutlined";
import DomainAddOutlinedIcon from '@mui/icons-material/DomainAddOutlined';
import PersonOutlinedIcon from "@mui/icons-material/PersonOutlined";
import AdminPanelSettingsOutlinedIcon from '@mui/icons-material/AdminPanelSettingsOutlined';
import SearchIcon from "@mui/icons-material/Search";
import HomeOutlinedIcon from "@mui/icons-material/HomeOutlined";
import LogoutOutlinedIcon from '@mui/icons-material/LogoutOutlined';
import LoginOutlinedIcon from '@mui/icons-material/LoginOutlined';
import useMediaQuery from '@mui/material/useMediaQuery';
import { useNavigate, useLocation } from 'react-router-dom';
import Cookies from 'js-cookie';
import { jwtDecode } from 'jwt-decode';
import Filter from "./Filter";


const TopBar = ({ setQueryString, queryString, fetchData }) => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down(550));
    const colors = tokens(theme.palette.mode);
    const colorMode = useContext(ColorModeContext);
    const [isAdmin, setIsAdmin] = useState(false);
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [filterData, setFilterData] = useState({});
    const navigate = useNavigate();
    const location = useLocation();
    const isHomePage = location.pathname === '/';

    const handleLogout = () => {
        setIsLoggedIn(false);
        document.cookie = "jwt=; max-age=0; path=/; domain=.localhost; SameSite=None; Secure";
        document.cookie = "email=; max-age=0; path=/; domain=.localhost; SameSite=None; Secure";
        navigate('/');
        window.location.reload();
    };

    const fetchDataForFilters = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_BACKEND_URL}/api/business/filters`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
    
            const data = await response.json();
    
            if (!response.ok) {
              return;
            }
    
            setFilterData(data);
            
        } catch (error) {
        }
      };

    useEffect(() => {
        if (Cookies.get('jwt')) {
            setIsLoggedIn(true);
            const decodedToken = jwtDecode(Cookies.get('jwt'));
            if (!decodedToken || !decodedToken.exp){
                handleLogout();
            }else{
                setIsAdmin(decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === 'Admin' ? true : false);
            }
        }

        fetchDataForFilters();
    }, []);

    return (
        <>
            <Box display='flex' justifyContent='space-between' p={2}>
                {/*FILTERS*/}
                <Box display='flex' sx={{ width: '30%', marginBottom: '10px' }}>
                    <Filter width='33%' 
                            label='odaberi državu' 
                            data={filterData.countries} 
                            setQueryString={setQueryString} 
                            queryString={queryString}
                            logicParameter={1}
                            fetchData={fetchData}
                            disabled={!isHomePage}
                            />
                    <Filter width='33%' 
                            label='odaberi grad' 
                            data={filterData.towns} 
                            setQueryString={setQueryString} 
                            queryString={queryString}
                            logicParameter={2}
                            fetchData={fetchData}
                            disabled={!isHomePage}
                            />
                    <Filter width='33%' 
                            label='odaberi uslugu' 
                            data={filterData.businessTypes} 
                            setQueryString={setQueryString} 
                            queryString={queryString}
                            logicParameter={3}
                            fetchData={fetchData}
                            disabled={!isHomePage}
                            />
                </Box>

                {/*SEARCH BAR*/}
                <Box display='flex' backgroundcolor={colors.primary[400]} borderradius='10px' width="20%" sx={{ marginTop: '10px' }} >
                    <InputBase sx={{
                        ml: 2,
                        flex: 1,
                        fontSize: '12px',
                        padding: '5px',
                    }} placeholder="Pretraži" />

                    <IconButton type="button" sx={{ p: 1 }}>
                        <SearchIcon sx={{ fontSize: isMobile ? 10 : 20 }} />
                    </IconButton>
                </Box>
                
                {/*ICONS*/}
                <Box display='flex' sx={{ width: isMobile ? '50%' : '30%', justifyContent: 'right', marginTop: '7px' }} >
                    {isAdmin && 
                        <IconButton onClick={() => navigate('/admin')}>
                            <AdminPanelSettingsOutlinedIcon
                                sx={{ fontSize: isMobile ? 10 : 20 }}
                            />
                        </IconButton>
                    }

                    <IconButton onClick={() => navigate('/')}>
                        <HomeOutlinedIcon
                            sx={{ fontSize: isMobile ? 10 : 20 }}
                        />
                    </IconButton>

                    <IconButton onClick={colorMode.toggleColorMode} type="button" sx={{ p: 1 }}>
                        {theme.palette.mode === 'dark' ? (
                            <DarkModeOutlinedIcon sx={{ fontSize: isMobile ? 10 : 20 }} />
                        ) : (
                            <LightModeOutlinedIcon sx={{ fontSize: isMobile ? 10 : 20 }} />
                        )}
                    </IconButton>
                    
                    {/*
                    <IconButton type="button" sx={{ p: 1 }}>
                        <NotificationsOutlinedIcon sx={{ fontSize: 20 }} />
                    </IconButton>

                    
                    <IconButton type="button" sx={{ p: 1 }} onClick={handleClick}>
                        <SettingsOutlinedIcon sx={{ fontSize: 20 }} />
                    </IconButton>
                    */}

                    {isLoggedIn && <IconButton type="button" onClick={() => navigate('/user/accountsettings')} sx={{ p: 1 }}>
                        <PersonOutlinedIcon sx={{ fontSize: isMobile ? 10 : 20 }} />
                    </IconButton>
                    }

                    {isLoggedIn && <IconButton type="button" onClick={() => navigate('/registernewbusiness')} sx={{ p: 1 }}>
                        <DomainAddOutlinedIcon sx={{ fontSize: isMobile ? 10 : 20 }} />
                    </IconButton>
                    }

                    {isLoggedIn ?
                        <IconButton onClick={handleLogout}>
                            <LogoutOutlinedIcon
                                sx={{ fontSize: isMobile ? 10 : 20 }}
                            />
                        </IconButton>
                        :
                        <IconButton onClick={() => navigate('/login')}>
                            <LoginOutlinedIcon
                                sx={{ fontSize: isMobile ? 10 : 20 }}
                            />
                        </IconButton>
                    }
                </Box>
            </Box>
        </>
        
    );
}

export default TopBar;