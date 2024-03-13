import {
    PlusOutlined,
    LeftOutlined
} from '@ant-design/icons';
import {
    Button,
    Form,
    Input,
    InputNumber, message, Row, Spin,
    Upload, type UploadFile, type UploadProps,
} from 'antd';
import {useNavigate, useParams} from "react-router-dom";
import {editProductAsync, getImageAsync, getProductByIdAsync} from "../../../Shared/api.ts";
import React, {useEffect, useState} from "react";
import {arrayMove, horizontalListSortingStrategy, SortableContext, useSortable} from "@dnd-kit/sortable";
import {CSS} from "@dnd-kit/utilities";
import {DndContext, type DragEndEvent, PointerSensor, useSensor} from "@dnd-kit/core";
import styles from "../../../styles.module.css";
import IEditProductFormData from "../../../Interfaces/Component/IEditProductFormData.ts";
import {RcFile} from "antd/es/upload";
import {navigateAndSaveParams} from "../../Shared/NavigationExtensions.ts";

const {TextArea} = Input;

interface DraggableUploadListItemProps {
    originNode: React.ReactElement<any, string | React.JSXElementConstructor<any>>;
    file: UploadFile;
}

const DraggableUploadListItem = ({originNode, file}: DraggableUploadListItemProps) => {
    const {attributes, listeners, setNodeRef, transform, transition, isDragging} = useSortable({
        id: file.uid,
    });

    const style: React.CSSProperties = {
        transform: CSS.Transform.toString(transform),
        transition,
        cursor: 'move',
    };

    return (
        <div
            ref={setNodeRef}
            style={style}
            className={isDragging ? 'is-dragging' : ''}
            {...attributes}
            {...listeners}
        >
            {/* hide error tooltip when dragging */}
            {file.status === 'error' && isDragging ? originNode.props.children : originNode}
        </div>
    );
};

interface IInitializeData {
    isDefault: boolean
    name: string
    description: string
    price: number
}

const EditProduct = () => {
    const [initializeData, setInitializeData] = useState<IInitializeData>({
        isDefault: true,
        name: "",
        description: "",
        price: 0
    });
    const {categoryId: categoryIdString, id: idString} = useParams();
    const id = parseInt(idString!);
    const navigate = useNavigate();
    const [form] = Form.useForm<IEditProductFormData>();
    const [messageApi, contextHolder] = message.useMessage();
    const [processing, setProcessing] = useState(false);
    const [imageList, setImageList] = useState<UploadFile[]>([]);

    const backUrl = `/category/${categoryIdString}`;

    const loadData = async () => {
        setProcessing(true);

        try {
            const product = await getProductByIdAsync(id);

            const blobs: Blob[] = [];
            for (const i of product.images) {
                blobs.push(await getImageAsync(i));
            }

            const images = blobs.map((b, i): UploadFile => {
                return {
                    uid: i.toString(),
                    name: product.images[i],
                    status: 'done',
                    originFileObj: new File([b], product.images[i]) as RcFile,
                    url: URL.createObjectURL(b)
                };
            });

            setImageList(images);

            setInitializeData({
                isDefault: false,
                name: product.name,
                description: product.description,
                price: product.price
            });
        } catch (error: any) {
            messageApi.error("Failed to load data");
        }

        setProcessing(false);
    }

    useEffect(() => {
        loadData();
    }, []);

    const onFinish = async (value: IEditProductFormData) => {
        setProcessing(true);

        const form = new FormData();
        form.append("id", idString!);
        form.append("name", value.name);
        form.append("description", value.description);
        form.append("price", value.price?.toString()?.replace('.', ','));
        for (const image of imageList)
            form.append("images", image.originFileObj as Blob);
        form.append("categoryId", categoryIdString!);

        try {
            await editProductAsync(form);
            navigateAndSaveParams(navigate, backUrl);
        } catch (error: any) {
            messageApi.error(`${error.response.data.title || error.response.data}`);
        }

        setProcessing(false);
    };

    const sensor = useSensor(PointerSensor, {
        activationConstraint: {distance: 10},
    });

    const onDragEnd = ({active, over}: DragEndEvent) => {
        if (active.id !== over?.id) {
            setImageList((prev) => {
                const activeIndex = prev.findIndex((i) => i.uid === active.id);
                const overIndex = prev.findIndex((i) => i.uid === over?.id);
                return arrayMove(prev, activeIndex, overIndex);
            });
        }
    };

    const onChange: UploadProps['onChange'] = ({fileList: newFileList}) => {
        setImageList(newFileList);
    };

    return (
        <>
            {contextHolder}

            <Row className={`${styles.topMenu}`}>
                <Button type="primary" onClick={() => navigateAndSaveParams(navigate, backUrl)}>
                    <LeftOutlined/>
                    <span>Return</span>
                </Button>
            </Row>

            <Spin spinning={processing}>
                <div style={{
                    display: 'flex',
                    justifyContent: 'center',
                    paddingTop: 20,
                    minHeight: '300px'
                }}>
                    {!initializeData.isDefault &&
                        <Form
                            form={form}
                            onFinish={onFinish}
                            layout="vertical"
                            style={{
                                maxWidth: '500px',
                                width: '100%',
                            }}
                        >
                            <Form.Item
                                label="Name"
                                name="name"
                                initialValue={initializeData.name}
                                rules={[
                                    {required: true, message: 'Enter the name'},
                                    {max: 100, message: 'Name is too long'}
                                ]}
                            >
                                <Input/>
                            </Form.Item>

                            <Form.Item>
                                <DndContext
                                    sensors={[sensor]}
                                    onDragEnd={onDragEnd}
                                >
                                    <SortableContext
                                        items={imageList.map((i) => i.uid)}
                                        strategy={horizontalListSortingStrategy}>
                                        <Form.Item
                                            label="Images"
                                            name="images"
                                            rules={
                                                [
                                                    {
                                                        validator: (rule, value, callback) => {
                                                            if (!imageList || imageList.length == 0) {
                                                                callback('Upload at least one image');
                                                                return;
                                                            }

                                                            callback();
                                                        }
                                                    }
                                                ]
                                            }
                                        >
                                            <Upload
                                                beforeUpload={() => false}
                                                accept="image/*"
                                                listType="picture-card"
                                                fileList={imageList}
                                                onChange={onChange}
                                                itemRender={(originNode, file) => (
                                                    <DraggableUploadListItem originNode={originNode} file={file}/>
                                                )}
                                            >
                                                <button style={{border: 0, background: 'none'}} type="button">
                                                    <PlusOutlined/>
                                                    <div style={{marginTop: 8}}>Upload</div>
                                                </button>
                                            </Upload>
                                        </Form.Item>
                                    </SortableContext>
                                </DndContext>
                            </Form.Item>

                            <Form.Item
                                label="Description"
                                name="description"
                                initialValue={initializeData.description}
                                rules={[
                                    {required: true, message: 'Enter the description'},
                                    {max: 4000, message: 'Description is too long'}
                                ]}
                            >
                                <TextArea/>
                            </Form.Item>

                            <Form.Item
                                label="Price"
                                name="price"
                                initialValue={initializeData.price}
                                rules={[
                                    {required: true, message: 'Enter the price'},
                                    {
                                        validator: (_, value, callback) => {
                                            if (value == null || Number(value) < 0) {
                                                callback('Price cannot be negative');
                                            }

                                            callback();
                                        }
                                    }
                                ]}
                            >
                                <InputNumber
                                    precision={2}
                                    style={{
                                        width: '100%'
                                    }}
                                />
                            </Form.Item>

                            <Form.Item>
                                <Button type="primary" htmlType="submit" style={{
                                    width: '100%'
                                }}>
                                    Save
                                </Button>
                            </Form.Item>
                        </Form>
                    }
                </div>
            </Spin>
        </>
    );
};

export default EditProduct;