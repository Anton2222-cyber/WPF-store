import {useState} from 'react';
import Box from '@mui/material/Box';
import Modal from '@mui/material/Modal';
import Button from '@mui/material/Button';
import {deleteCategoryAsync} from '../../Shared/api.ts';
import style from '../../modalStyles.ts';

interface IDeleteCategoryModalProps {
    isOpenModal: boolean,
    id: number,
    closeModal: () => void,
    reloadCategories: () => void
}

const DeleteCategoryModal = (props: IDeleteCategoryModalProps) => {
    const [errorMessage, setErrorMessage] = useState<string>('');

    const {isOpenModal, id, closeModal, reloadCategories} = props;

    const handleCloseDeleteModal = () => {
        setErrorMessage('');

        closeModal();
    }

    const deleteCategoryHandler = async () => {
        try {
            await deleteCategoryAsync(id);
            reloadCategories();
            setErrorMessage('');
            handleCloseDeleteModal();
        } catch (error: any) {
            setErrorMessage(`Error: ${error.message}.`);
        }
    }

    return (
        <Modal
            open={isOpenModal}
            onClose={handleCloseDeleteModal}
        >
            <Box sx={{...style}}>
                <h2>Видалення категорії</h2>
                <p className='text-danger'>Дійсно видалити категорію?</p>

                <button onClick={deleteCategoryHandler} className='btn btn-danger'>Delete</button>
                <Button onClick={handleCloseDeleteModal}>Сlose</Button>
                {errorMessage && <span className='text-danger'>{errorMessage}</span>}
            </Box>
        </Modal>
    );
}

export default DeleteCategoryModal;