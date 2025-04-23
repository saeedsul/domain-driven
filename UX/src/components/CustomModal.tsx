import React from 'react';
import { Modal} from 'react-bootstrap';

type ModalProps = {
    isOpen: boolean;
    onClose: () => void;
    title: string;
    body: React.ReactNode;
};

const CustomModal: React.FC<ModalProps> = ({ isOpen, onClose, title, body }) => {
    return (
        <Modal show={isOpen} onHide={onClose} centered backdrop="static">
            <Modal.Header closeButton>
                <Modal.Title>{title}</Modal.Title>
            </Modal.Header>
            <Modal.Body>{body}</Modal.Body> 
        </Modal>
    );
};

export default CustomModal;
