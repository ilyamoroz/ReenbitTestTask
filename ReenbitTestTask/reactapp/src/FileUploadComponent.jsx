import React, { useState } from 'react';
import './FileUploadComponent.css'

function FileUploadComponent() {
    const [file, setFile] = useState(null);
    const [email, setEmail] = useState('');
    const [isEmailValid, setIsEmailValid] = useState(true);
    const [isFileValid, setIsFileValid] = useState(true);

    const handleFileChange = (event) => {

        const selectedFile = event.target.files[0];
        console.log(selectedFile?.type)
        const allowedTypes = ["application/vnd.openxmlformats-officedocument.wordprocessingml.document"];
        if (!allowedTypes.includes(selectedFile?.type)) {
            setIsFileValid(false);
            return;
        } else {
            setIsFileValid(true);
            setFile(selectedFile);
        }
        console.log(isFileValid)
        
    };

    const handleEmailChange = (event) => {
        setEmail(event.target.value);
    };

    const handleSubmit = async (event) => {
        event.preventDefault();

        setIsEmailValid(validateEmail(email));

        console.log(isEmailValid);
        if (isEmailValid && isFileValid) {
            const formData = new FormData();
            formData.append('file', file);
            try {
                const response = await fetch(`https://localhost:7150/api/File/Upload?email=${email}`, {
                    method: 'POST',
                    body: formData,
                });
                const data = await response.json();
                console.log('Upload successful:', data);
            } catch (error) {
                console.error('Error uploading file:', error);
            }
        }
        

    };

    return (
        <div className='form-container'>
            <form className='form'>
                <div className='input-container'>
                    <input className='input-email' accept=".docx" type="email" name="email" placeholder='Enter your email' required onChange={handleEmailChange} />
                    <input className='input-file' id='file' type="file" onChange={handleFileChange} />
                    <label htmlFor="file">
                        <img className='upload-img' src="./upload-file.svg" alt="Upload" />
                    </label>
                </div>
                {!isEmailValid && <p style={{ color: 'red' }}>Please enter a valid email address</p>}
                {!isFileValid && <p style={{ color: 'red' }}>Only .docx files are allowed.</p>}
                <button className='upload-btn' type='submit' onClick={handleSubmit}>Upload</button>
            </form>
        </div>
    );
}

const validateEmail = (email) => {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
};

export default FileUploadComponent;
