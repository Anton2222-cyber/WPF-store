import {ReactNode, useState} from "react";
import {DeleteOutlined, MinusOutlined, PlusOutlined} from "@ant-design/icons";
import {Card, Col, message, Spin} from "antd";
import {deleteBasketProductAsync, imagesDir, setQuantityAsync} from "../../Shared/api.ts";
import Meta from "antd/es/card/Meta";
import IBasketItem from "../../Interfaces/Api/IBasketItem.ts";

interface IBasketItemProps {
    basketProduct: IBasketItem,
    reloadItems: () => void
}

export function BasketItem(props: IBasketItemProps) {
    const {basketProduct, reloadItems} = props;
    const [quantity, setQuantity] = useState(basketProduct.quantity);
    const [processing, setProcessing] = useState(false);
    const [messageApi, contextHolder] = message.useMessage();

    const setQuantityToItem = async (quantity: number) => {
        setProcessing(true);

        try {
            await setQuantityAsync(basketProduct.product.id, quantity);
            setQuantity(quantity);
        } catch (error: any) {
            messageApi.error('Error');
        }

        setProcessing(false);
    }
    const increase = () => setQuantityToItem(quantity + 1);
    const decrease = () => {
        if (quantity > 1)
            setQuantityToItem(quantity - 1);
    }
    const deleteItem = async () => {
        setProcessing(true);

        try {
            await deleteBasketProductAsync(basketProduct.product.id);
            reloadItems();
        } catch (error: any) {
            messageApi.error('Error');
        }

        setProcessing(false);
    }

    const actions: ReactNode[] = [
        <div key="minus" onClick={decrease}>
            <MinusOutlined/>
        </div>,
        <div key="quantity">
            {quantity}
        </div>,
        <div key="plus" onClick={increase}>
            <PlusOutlined/>
        </div>,
        <div key="delete" onClick={deleteItem}>
            <DeleteOutlined/>
        </div>,
        <div key="price">
            {`${basketProduct.product.price * quantity} UAH`}
        </div>
    ];


    return (
        <>
            {contextHolder}

            <Spin spinning={processing}>
                <Col span={8} style={{marginTop: '10px', marginBottom: '10px'}}>
                    <Card
                        hoverable
                        cover={<img alt="image" src={`${imagesDir}${basketProduct.product.images[0]}`}/>}
                        actions={actions}
                    >
                        <Meta title={basketProduct.product.name} description={basketProduct.product.description}/>
                    </Card>
                </Col>
            </Spin>
        </>
    );
}