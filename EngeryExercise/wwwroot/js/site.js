const uri = '/meter-reading-uploads';


async function uploadExcelFile() {
    const fileContext = document.getElementById('fileInput');
    //get file data
    const fileData = fileContext.files[0];
    const formData  = new FormData();

    formData.append('UploadFiles', fileData);

    if (fileContext.files.length > 0) {
        try {
            const response = await fetch(uri, {
                method: 'POST',
                body: formData,
            });

            if (!response.ok) {
                document.getElementById("responseMessage").innerHTML = `HTTP error! Status: ${response.status}`;
            }

            const result = await response.json();
            //display missage
            const responseContent = document.getElementById('responseMessage');
            responseContent.innerHTML = `<p>${result.message}</p>`;
        }
        catch (error) {
            document.getElementById("responseMessage").innerHTML = 'Error during API request: ' + error;
        }

    }
    else {
        alert('Please select excel file');
    }
}