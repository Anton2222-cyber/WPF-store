import { Formik, Form, Field, ErrorMessage } from 'formik';
import { createCategoryAsync } from '../../Shared/api.ts';
import { useState, useRef } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import ICreateCategoryData from '../../Interfaces/Formik/ICreateCategoryData.tsx';
import styles from '../../styles.module.css';

interface IAddCategoryFormProps {
	hide: () => void
}

const AddCategoryForm = (props: IAddCategoryFormProps) => {
	const [errorMessage, setErrorMessage] = useState<string>('');
	let image = useRef<File | null>(null);

	const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		const fileInput = event.target;
		if (fileInput.files && fileInput.files.length > 0) {
			const selectedFile = fileInput.files[0];
			image.current = selectedFile;
		} else {
			image.current = null;
		}
	};

	return (
		<Formik<ICreateCategoryData>
			initialValues={{
				name: '',
				description: ''
			}}

			validate={(values: ICreateCategoryData) => {
				const errors = {} as any;
				const { name, description } = values;
				if (!name)
					errors.name = 'Required';
				if (name.startsWith(' '))
					errors.name = 'Starts with space symbol';
				if (name.endsWith(' '))
					errors.name = 'Ends with space symbol';
				if (name.length > 200)
					errors.name = 'Too long name';

				if (!image.current)
					errors.image = 'Required';

				if (!description)
					errors.description = 'Required';
				if (description.startsWith(' '))
					errors.description = 'Starts with space symbol';
				if (description.endsWith(' '))
					errors.description = 'Ends with space symbol';
				if (description.length > 4000)
					errors.name = 'Too long description';

				return errors;
			}}

			onSubmit={async (values, { setSubmitting }) => {
				const { name, description } = values;

				const formData = new FormData();
				formData.append('name', name);
				if (image.current) formData.append('image', image.current);
				formData.append('description', description);

				try {
					await createCategoryAsync(formData);
					setErrorMessage('');
					props.hide();
				} catch (error: any) {
					setErrorMessage(`Error: ${error.message}. Text: ${error.response.data.title || error.response.data}`);
				}

				setSubmitting(false);
			}}
		>
			{({ handleSubmit, isSubmitting }) => (
				<Form onSubmit={handleSubmit} className={`container-fluid ${styles.darkBorder}`} style={{ paddingBottom: "1rem" }}>
					<div className="form-group">
						<label htmlFor="nameInput">Name</label>
						<Field name='name' type='text' className="form-control" id="nameInput" />
						<ErrorMessage name="name" component="div" className='text-danger' />

						<label htmlFor="imageInput">Image</label>
						<Field name='image' type="file" className="form-control" id="imageInput"
							onChange={handleFileChange} />
						<ErrorMessage name="image" component="div" className='text-danger' />

						<label htmlFor="descriptionInput">Description</label>
						<Field name='description' className="form-control" id="descriptionInput" as="textarea" />
						<ErrorMessage name="description" component="div" className='text-danger' />
					</div>
					<button type="submit" disabled={isSubmitting} className="btn btn-primary"
						style={{ marginTop: "1rem" }}>Create
					</button>
					{errorMessage && <span className='text-danger'>{errorMessage}</span>}
				</Form>
			)}
		</Formik>
	);
}

export default AddCategoryForm;