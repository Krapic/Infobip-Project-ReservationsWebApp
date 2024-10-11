const ErrorComponent = ({ errorMessage }) => {

  return (
    <div style={{ height: '80%', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
        <p style={{ font: 'Arial', fontSize: '20px', textAlign: 'center' }}>{errorMessage}</p>
    </div>
  );
};
  
export default ErrorComponent;