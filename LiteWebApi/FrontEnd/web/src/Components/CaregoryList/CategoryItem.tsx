import {ReactNode, useState} from 'react';
import Meta from 'antd/es/card/Meta';
import {Card, Col} from 'antd';
import {imagesDir} from '../../Shared/api.ts';
import ICategory from '../../Interfaces/Api/ICategory.tsx';
import {useAppSelector} from "../../Redux/Hooks/hooks.ts";
import {useNavigate} from "react-router-dom";
import {EnterOutlined, EditOutlined, DeleteOutlined} from '@ant-design/icons';
import EditCategoryModal from "./EditCategoryModal.tsx";
import DeleteCategoryModal from "./DeleteCategoryModal.tsx";
import {navigateAndSetParams} from "../Shared/NavigationExtensions.ts";

interface ICategoryItemProps {
    category: ICategory,
    reloadCategories: () => void
}

const CategoryItem = (props: ICategoryItemProps) => {
    const navigate = useNavigate();
    const [isOpenedEditModal, setIsOpenedEditModal] = useState<boolean>(false);
    const [isOpenedDeleteModal, setIsOpenedDeleteModal] = useState<boolean>(false);
    const {user} = useAppSelector(store => store.auth);


    const visit = () => navigateAndSetParams(navigate, `/category/${props.category.id}`, {
        productOffset: undefined
    });

    const defaultActions: ReactNode[] = [
        <div key="enter" onClick={visit}>
            <EnterOutlined/>
        </div>
    ];
    const adminActions: ReactNode[] = [
        <div key="edit" onClick={() => setIsOpenedEditModal(true)}>
            <EditOutlined/>
        </div>,
        <div key="delete" onClick={() => setIsOpenedDeleteModal(true)}>
            <DeleteOutlined/>
        </div>
    ];

    const actions = user?.hasRole('Admin') ? defaultActions.concat(adminActions) : defaultActions;

    const {category: {id}, reloadCategories} = props;

    return (
        <>
            <EditCategoryModal id={id} isOpenModal={isOpenedEditModal}
                               closeModal={() => setIsOpenedEditModal(false)}
                               reloadCategories={reloadCategories}/>
            <DeleteCategoryModal id={id} isOpenModal={isOpenedDeleteModal}
                                 closeModal={() => setIsOpenedDeleteModal(false)}
                                 reloadCategories={reloadCategories}/>

            <Col span={8}>
                <Card
                    hoverable
                    cover={<img alt="image" src={`${imagesDir}${props.category.image}`}
                                onClick={visit}/>}
                    actions={actions}
                >
                    <Meta title={props.category.name} description={props.category.description}/>
                </Card>
            </Col>
        </>
    );
}

export default CategoryItem;
