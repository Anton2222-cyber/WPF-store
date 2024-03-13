import IOrder from "../../Interfaces/Api/IOrder.ts";
import {Avatar, Card} from "antd";
import Meta from "antd/es/card/Meta";
import {imagesDir} from "../../Shared/api.ts";
import ICustomerOrder from "../../Interfaces/Api/ICustomerOrder.ts";
import ICustomer from "../../Interfaces/Api/ICustomer.ts";

interface IOrderProps {
    order: IOrder
}

function OrderItem(props: IOrderProps | ICustomerOrder) {
    if (!("order" in props))
        throw new Error("Props must contain 'order' property");

    const {id, status, orderedProducts, timeOfCreation, postOffice} = props.order;

    const customer: ICustomer | null = "customer" in props.order ? props.order.customer as ICustomer : null;

    const formatDate = (date: Date) =>
        new Date(date).toLocaleDateString('uk-UA', {
            weekday: "long",
            year: "numeric",
            month: "short",
            day: "numeric"
        });

    const dateTime = formatDate(new Date(timeOfCreation))
    const statusDateTime = formatDate(new Date(status.timeOfCreation))

    return (
        <Card
            title={dateTime}
            style={{
                margin: 20
            }}
            extra={<p>{`Id: ${id}`}</p>}
        >
            <Card
                title="Details"
                style={{
                    width: 600,
                    marginTop: 16,
                }}
            >
                <div>
                    <p>{`Status: ${status.status}`}</p>
                    <p>{`Status changed: ${statusDateTime}`}</p>
                </div>
                <p>{`Shipping to: ${postOffice.name}`}</p>
            </Card>

            {customer && <Card
                title="Customer"
                style={{
                    width: 600,
                    marginTop: 16,
                }}
            >
                <Meta
                    avatar={<Avatar src={`${imagesDir}${customer.photo}`}/>}
                    title={`${customer.firstName} ${customer.lastName}`}
                />
            </Card>}

            {orderedProducts.map(op =>
                <Card
                    key={op.id}
                    style={{
                        width: 600,
                        marginTop: 16,
                    }}
                >
                    <Meta
                        avatar={<Avatar src={`${imagesDir}${op.product.images[0]}`}/>}
                        title={op.product.name}
                        description={`Unit price: ${op.unitPrice}. Quantity: ${op.quantity}`}
                    />
                </Card>
            )}
        </Card>
    )
}

export default OrderItem;