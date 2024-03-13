import Box from '@mui/material/Box';
import Modal from '@mui/material/Modal';
import Button from '@mui/material/Button';
import {ErrorMessage, Field, Form, Formik} from 'formik';
import IEditCategoryData from '../../Interfaces/Formik/IEditCategoryData.tsx';
import {editCategoryAsync} from '../../Shared/api.ts';
import {useState} from 'react';
import style from '../../modalStyles.ts';

interface IEditCategoryModalProps {
    isOpenModal: boolean,
    id: number,
    closeModal: () => void,
    reloadCategories: () => void
}

const EditCategoryModal = (props: IEditCategoryModalProps) => {
    const {id, isOpenModal, closeModal, reloadCategories} = props;

    const [image, setImage] = useState<File | null>();
    const [errorMessage, setErrorMessage] = useState<string>();

    const handleCloseModal = () => {
        setErrorMessage('');
        setImage(null);

        closeModal();
    }

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const fileInput = event.target;
        if (fileInput.files && fileInput.files.length > 0) {
            const selectedFile = fileInput.files[0];
            setImage(selectedFile);
        } else {
            setImage(null);
        }
    };

    const validateEditData = (values: IEditCategoryData) => {
        const errors = {} as any;
        const {name, description} = values;

        if (name.startsWith(' '))
            errors.name = 'Starts with space symbol';
        if (name.endsWith(' '))
            errors.name = 'Ends with space symbol';
        if (name.length > 200)
            errors.name = 'Too long name';

        if (description.startsWith(' '))
            errors.description = 'Starts with space symbol';
        if (description.endsWith(' '))
            errors.description = 'Ends with space symbol';
        if (description.length > 4000)
            errors.name = 'Too long description';

        return errors;
    }

    const editForm = (
        <Formik<IEditCategoryData>
            initialValues={{
                name: '',
                description: ''
            }}

            validate={validateEditData}

            onSubmit={async (values, {setSubmitting}) => {
                const {name, description} = values;

                const formData = new FormData();
                formData.append('id', id.toString());
                formData.append('name', name);
                if (image) formData.append('image', image);
                formData.append('description', description);

                try {
                    await editCategoryAsync(formData);
                    reloadCategories();
                    setErrorMessage('');
                    handleCloseModal();
                } catch (error: any) {
                    setErrorMessage(`Error: ${error.message}. Text: ${error.response.data.title || error.response.data}`);
                }

                setSubmitting(false);
            }}
        >
            {({handleSubmit, isSubmitting}) => (
                <Form onSubmit={handleSubmit} className='container-fluid' style={{paddingBottom: "1rem"}}>
                    <div className="form-group">
                        <label htmlFor="nameInput">Name</label>
                        <Field name='name' type='text' className="form-control" id="nameInput"/>
                        <ErrorMessage name="name" component="div" className='text-danger'/>

                        <label htmlFor="imageInput">Image</label>
                        <Field name='image' type="file" className="form-control" id="imageInput"
                               onChange={handleFileChange}/>
                        <ErrorMessage name="image" component="div" className='text-danger'/>

                        <label htmlFor="descriptionInput">Description</label>
                        <Field name='description' className="form-control" id="descriptionInput" as="textarea"/>
                        <ErrorMessage name="description" component="div" className='text-danger'/>
                    </div>
                    <button type="submit" disabled={isSubmitting} className="btn btn-primary"
                            style={{marginTop: "1rem"}}>Edit
                    </button>
                    {errorMessage && <span className='text-danger'>{errorMessage}</span>}
                </Form>
            )}
        </Formik>
    );


    return (
        <Modal
            open={isOpenModal}
            onClose={handleCloseModal}
        >
            <Box sx={{...style}}>
                <h2>Рудагування категорії</h2>
                <p>Вводьте дані лише в поля, які потрібно редагувати</p>

                {editForm}
                <Button onClick={handleCloseModal}>Сlose</Button>
            </Box>
        </Modal>
    );
}

export default EditCategoryModal;