import {ReactNode, useState} from 'react';
import Meta from 'antd/es/card/Meta';
import {Card, Col, Spin, message} from 'antd';
import {addToBasketAsync, imagesDir} from '../../../Shared/api.ts';
import IProduct from '../../../Interfaces/Api/IProduct.ts';
import {useAppSelector} from "../../../Redux/Hooks/hooks.ts";
import {useNavigate, useParams} from "react-router-dom";
import {DeleteOutlined, EditOutlined, EnterOutlined, ShoppingCartOutlined} from "@ant-design/icons";
import {navigateAndSaveParams} from "../../Shared/NavigationExtensions.ts";

interface IProductItemProps {
    product: IProduct
}

const ProductItem = (props: IProductItemProps) => {
    const navigate = useNavigate();
    const [isProcessing, setIsProcessing] = useState<boolean>(false);
    const [messageApi, contextHolder] = message.useMessage();
    const {categoryId: categoryIdString} = useParams();
    const {user} = useAppSelector(store => store.auth);

    const visit = () => navigateAndSaveParams(navigate, `/category/${categoryIdString}/product/${props.product.id}`);
    const edit = () => navigateAndSaveParams(navigate, `/category/${categoryIdString}/edit-product/${props.product.id}`);

    const addToBasket = async () => {
        setIsProcessing(true);

        try {
            await addToBasketAsync(props.product.id);
            messageApi.success('Added to basket');
        } catch (error: any) {
            if (error.response.data == 'Product is already in basket')
                messageApi.info('Already in the basket');
            else
                messageApi.error('Error');
        }

        setIsProcessing(false);
    }

    const defaultActions: ReactNode[] = [
        <div key="enter" onClick={visit}>
            <EnterOutlined/>
        </div>
    ];
    const userActions: ReactNode[] = [
        <div key="buy" onClick={addToBasket}>
            <ShoppingCartOutlined/>
        </div>,
    ]
    const adminActions: ReactNode[] = [
        <div key="edit" onClick={edit}>
            <EditOutlined/>
        </div>,
        <div key="delete">
            <DeleteOutlined/>
        </div>
    ];

    let actions = defaultActions;
    if (user?.hasRole('User') || user?.hasRole('Admin'))
        actions = actions.concat(userActions)
    if (user?.hasRole('Admin'))
        actions = actions.concat(adminActions)

    return (
        <>
            {contextHolder}

            <Col span={8}>
                <Spin spinning={isProcessing} size="large">
                    <Card
                        hoverable
                        cover={<img alt="image" src={`${imagesDir}${props.product.images[0]}`} onClick={visit}/>}
                        actions={actions}
                    >
                        <Meta title={props.product.name} description={props.product.description}/>
                    </Card>
                </Spin>
            </Col>
        </>
    );
}

export default ProductItem;
