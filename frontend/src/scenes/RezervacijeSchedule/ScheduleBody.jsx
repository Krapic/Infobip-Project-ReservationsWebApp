import { ScheduleComponent, Week, Inject, ViewsDirective, ViewDirective } from '@syncfusion/ej2-react-schedule';
import { DateTimePickerComponent } from '@syncfusion/ej2-react-calendars';
import { DropDownListComponent } from '@syncfusion/ej2-react-dropdowns';
import { Internationalization } from '@syncfusion/ej2-base';
import { FormValidator } from '@syncfusion/ej2-inputs';
import { useRef, useEffect, useState } from 'react';
import { useTheme } from "@mui/material";
import Cookies from 'js-cookie';
import { jwtDecode } from 'jwt-decode';
import './ScheduleBody.css';


const ScheduleBody = (propsData) => {
    const jwt = Cookies.get('jwt'); 
    const theme = useTheme();
    const scheduleObj = useRef(null);
    const [workingDays, setWorkingDays] = useState([1, 2, 3, 4, 5, 6, 7]);
    const [userName, setUserName] = useState('');
    const [userEmail, setUserEmail] = useState("");

    useEffect(() => {
      const authToken = Cookies.get('jwt');
      setUserEmail(Cookies.get('email'));

      if (authToken) {
        const decodedToken = jwtDecode(authToken);
        setUserName(decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']);
      }

      let array = [];
      propsData.workHours.$values.forEach(e => {
        array.push(e.Day + 1);
      });
      setWorkingDays(array);

    }, []);

    /*Editor popup*/
    const onSaveButtonClick = async (args) => {
      const newAppointment = {
        Id: args.data.Id,
        Subject: userName,
        StartTime: args.element.querySelector('#StartTime').ej2_instances[0].value,
        EndTime: args.element.querySelector('#EndTime').ej2_instances[0].value,
        EventType: args.element.querySelector('#EventType').ej2_instances[0].value,
        IsAllDay: false,
        
      };

      if (args.target.classList.contains('e-appointment')) {
        if(userName !== args.data.Subject){
          alert("Ovo nije vaša rezervacija!");
        }else{
          scheduleObj.current.saveEvent(newAppointment, 'Save');
        
          try {
            const response = await fetch(`${process.env.REACT_APP_BACKEND_URL}/api/reservation/update-reservation`, {
                method: 'PATCH',
                headers: {
                    'Authorization' : `Bearer ${jwt}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                  BusinessName: propsData.businessName,
                  UserEmail: userEmail ?? "",
                  Subject: newAppointment.Subject,
                  StartTime: newAppointment.StartTime,
                  EndTime: newAppointment.EndTime,
                  EventType: newAppointment.EventType
                })
            });
  
            const jsonResponse = await response.json();
  
            if (!response.ok) {
              throw new Error(jsonResponse.response);
            }
  
          } catch (error) {
            //TODO generirat komponentu za error
          }
        }

      } else {
        newAppointment.Id = scheduleObj.current.getEventMaxID();
        scheduleObj.current.addEvent(newAppointment);

        try {
          const response = await fetch(`${process.env.REACT_APP_BACKEND_URL}/api/reservation/reserve`, {
              method: 'POST',
              headers: {
                  'Authorization' : `Bearer ${jwt}`,
                  'Content-Type': 'application/json'
              },
              body: JSON.stringify({
                BusinessName: propsData.businessName,
                UserEmail: userEmail ?? "",
                Subject: newAppointment.Subject,
                StartTime: newAppointment.StartTime,
                EndTime: newAppointment.EndTime,
                EventType: newAppointment.EventType
                
                
              })
          });

          const jsonResponse = await response.json();

          if (!response.ok) {
            throw new Error(jsonResponse.response);
          }

        } catch (error) {
          //TODO generirat komponentu za error
        }
      }

      scheduleObj.current.closeEditor();
    }

    const onPopupOpen = (args) => {
      args.duration = 30;

      if (args.type === 'Editor') {
          setTimeout(() => {
              const saveButton = args.element.querySelector('#Save');
              const cancelButton = args.element.querySelector('#Cancel');
              saveButton.onclick = (event) => {
                if (!formValidator.validate()) {
                  event.preventDefault();
                }else{
                    const startTime = args.element.querySelector('#StartTime').ej2_instances[0].value;
                    const endTime = args.element.querySelector('#EndTime').ej2_instances[0].value;

                    const isWithinWorkHours = propsData.workHours.$values.some(hour => {
                      
                        const [startHours, startMinutes] = hour.Start.split(':').map(Number);
                        let workHourStart = new Date(startTime);
                        workHourStart.setHours(startHours);
                        workHourStart.setMinutes(startMinutes);

                        const [endHours, endMinutes] = hour.End.split(':').map(Number);
                        let workHourEnd = new Date(endTime)
                        workHourEnd.setHours(endHours);
                        workHourEnd.setMinutes(endMinutes);

                        return hour.Highlight && (startTime >= workHourStart && endTime <= workHourEnd);
                    });

                    if (!isWithinWorkHours) {
                        alert('Rezervacije možete napraviti samo za vrijeme radnog vremena obrta!');
                        event.preventDefault();
                    } else {
                        onSaveButtonClick(args);
                    }
                }
              }
              cancelButton.onclick = () => {
                scheduleObj.current.closeEditor();
              };
          }, 100);

          let formElement = args.element.querySelector('.e-schedule-form');
          let formValidator = new FormValidator(formElement, {
            rules: {
                'EventType': { required: true }
            }
          });

      }
    }

    const editorTemplate = (props) => {
        return (props !== undefined ? 
        <table style={{ display: 'flex', justifyContent: 'center', marginTop: '5%' }} className="custom-event-editor">
          <tbody>
            <tr>
              <td style={{ marginBottom: '30px' }} className="e-textlabel">Aktivnost: </td>
              <td colSpan={4}>
                <DropDownListComponent 
                    id="EventType" 
                    placeholder='  Odaberi aktivnost' 
                    data-name="EventType" 
                    className="e-field"
                    dataSource={propsData.businessActivities.$values !== undefined ? propsData.businessActivities.$values.map(e => `${e.NameOfActivity} - ${e.Price}€`) : null}
                ></DropDownListComponent>
              </td>
            </tr>

            <tr style={{ height: '20px' }}></tr>

            <tr>
              <td className="e-textlabel">Početak rezervacije:</td>
              <td colSpan={4}>
                <DateTimePickerComponent format='dd/MM/yy hh:mm a' id="StartTime" data-name="StartTime" value={new Date(props.startTime || props.StartTime)} className="e-field" readonly></DateTimePickerComponent>
              </td>
            </tr>

            <tr style={{ height: '20px' }}></tr>

            <tr>
              <td className="e-textlabel">Kraj rezervacije:</td>
              <td colSpan={4}>
                <DateTimePickerComponent format='dd/MM/yy hh:mm a' id="EndTime" data-name="EndTime" value={new Date(props.endTime || props.EndTime)} className="e-field" readonly></DateTimePickerComponent>
              </td>
            </tr>

            <tr style={{ height: '20px' }}></tr>

            <tr>
              <td className="e-textlabel">Opis:</td>
              <td colSpan={4}>
                {propsData.businessActivities.$values.map((e, index) => 
                  (
                    <div key={index} style={{marginTop: '5px'}}>
                      <div>{`${e.NameOfActivity} - ${e.Price}€`}</div>
                      <div>{e.DescriptionOfActivity}</div>
                    </div>
                  )
                )}
              </td>
            </tr>

          </tbody>
        </table> 
        : 
        <div></div>);
    }

    const editorHeaderTemplate = (props) => {
        return (
          <div id="event-header" >
            {(props !== undefined) ? ((props.Subject) ? <div>Uredi rezervaciju:</div> : <div>Stvori novu rezervaciju:</div>) : <div></div>}
          </div>
        );
    }

    const editorFooterTemplate = () => {
        return (
          <div id="event-footer">
            <div id="right-button">
              <button id="Cancel" className="e-control e-btn e-secondary" data-ripple="true">
                Odustani
              </button>
              <button style={{ 
                  color: theme.palette.mode === 'dark' ? '#141B2D' : 'white',
                  backgroundColor: '#70D8BD'
                }} id="Save" className="e-control e-btn" data-ripple="true">
                Rezerviraj me!
              </button>
            </div>
          </div>
        );
    }
    
    /*Quick info popup*/

    const content = (props) => {
      return (
          <>
            {props.elementType !== "cell" ? (
                <div className="e-event-content e-template">
                    <div className="e-subject-wrap">
                        {props.EventType !== undefined ? (
                          <div>
                            <div className="location">{props.EventType}</div>
                            <div className="location">{propsData.businessActivities.$values.find(activity => `${activity.NameOfActivity} - ${activity.Price}€` === props.EventType).DescriptionOfActivity}</div>
                          </div>
                        ) : (
                            ""
                        )}
                    </div>
                </div>
            ) : (
                <div>
                    <p>Trajanje rezervacije: {getTimeString(props.StartTime)} - {getTimeString(props.EndTime)}</p>
                </div>
            )}
          </>
      );
    }

    

    const footer = (props) => {
        const onDeleteButtonClick = async () => {
          try {
            const response = await fetch(`${process.env.REACT_APP_BACKEND_URL}/api/reservation/cancel-reservation`, {
                method: 'DELETE',
                headers: {
                    'Authorization' : `Bearer ${jwt}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                  BusinessName: propsData.businessName,
                  UserEmail: userEmail ?? "",
                  Id: props.Id
                })
            });
  
            const jsonResponse = await response.json();
  
            if (!response.ok) {
              throw new Error(jsonResponse.response);
            }
  
          } catch (error) {
            //TODO generirat komponentu za error
          }

          scheduleObj.current.closeQuickInfoPopup();
          scheduleObj.current.deleteEvent(props.Id);
        };

        return (
          <>
            {props.elementType !== "cell" ? (
              <>
                <button style={{ backgroundColor: (props.Subject !== userName) ? '#cccccc' : '#70D8BD', color: 'white' }} id="delete" className="e-event-delete e-btn" data-ripple="true" onClick={onDeleteButtonClick} disabled={props.Subject !== userName} >Izbriši rezervaciju</button>
              </>
            ) : (
              <></>
            )}
          </>
        );
    }

    const instance = new Internationalization();

    const getTimeString = (value) => {
        return instance.formatDate(value, { skeleton: 'hm' });
    }

    const eventTemplate = (props) => {
        return (
        <div>
          <div className="subject">{props.Subject}</div>
          <div className="time">
            {getTimeString(props.StartTime)} - {getTimeString(props.EndTime)}
          </div>
          
        </div>);
    }

    /*Data*/
    const quickInfoTemplates = { content: content.bind(this), footer: footer.bind(this) };
    
    const eventSettings = { dataSource: propsData.appointments.$values, template: eventTemplate, enableMaxHeight: true };

    return (
      <>
        <div style={{ height: '35vh', overflowY: 'auto' }}>
          {theme.palette.mode === 'dark' ? 
            <link href="https://cdn.syncfusion.com/ej2/22.1.34/bootstrap5-dark.css" rel="stylesheet"/>
            :
            <link href="https://cdn.syncfusion.com/ej2/22.1.34/bootstrap5.css" rel="stylesheet"/>
          }
          
          <ScheduleComponent
              eventSettings={eventSettings}
              startHour={propsData.minimumHour}
              endHour={propsData.maximumHour}
              workHours={propsData.workHours}
              readonly={propsData.isReadonly}
              popupOpen={onPopupOpen}
              editorTemplate={editorTemplate.bind(this)}
              editorHeaderTemplate={editorHeaderTemplate}
              editorFooterTemplate={editorFooterTemplate}
              quickInfoTemplates={quickInfoTemplates}
              minDate={new Date().setDate(new Date().getDate() - 1)}
              workDays={workingDays}
              showWeekend={false}
              allowDragAndDrop={false}
              allowResizing={false}
              allowInline={false}
              ref={scheduleObj}
              >
              <ViewsDirective>
                  <ViewDirective option='Week' />
              </ViewsDirective>
              <Inject services={[Week]}/>
          </ScheduleComponent>
        </div>
      </>
    );
  };
  
  export default ScheduleBody;