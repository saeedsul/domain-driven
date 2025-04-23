import React from 'react';
import { Form, Button } from 'react-bootstrap';
import { AddActivityI, create } from '../../api/activity/activityApi';
import { handleRequest } from '../../utils/requestWrapper';
import domEventBus from '../../utils/domEventBus';

interface Props {
    onCreated: () => void;
}

const CreateActivity: React.FC<Props> = ({ onCreated }) => {
    const [formData, setFormData] = React.useState<AddActivityI>({
        name: '',
        fromAddress: '',
        toEmailAddress: '',
        fromName: '',
    });

    const [errors, setErrors] = React.useState<Record<string, string>>({});

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value,
        }));

        // Clear the error message as user types
        if (errors[name]) {
            setErrors(prev => ({
                ...prev,
                [name]: '',
            }));
        }
    };

    const validate = () => {
        const newErrors: Record<string, string> = {};
        Object.entries(formData).forEach(([key, value]) => {
            if (!value.trim()) {
                newErrors[key] = 'Field is required';
            }
        });
        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!validate()) return;

        const [response, error] = await handleRequest({ promise: create(formData) });

        if (error) {
            console.error('Create activity failed:', error);
        } else {
            console.log('Activity created:', response);
        }

        onCreated();
        domEventBus.dispatchEvent(new CustomEvent("activityCreated", { detail: {} }));
    };

    return (
        <Form onSubmit={handleSubmit}>
            <Form.Group className="mb-3">
                <Form.Label>Campaign Name</Form.Label>
                <Form.Control
                    name="name"
                    value={formData.name}
                    onChange={handleChange}
                    isInvalid={!!errors.name}
                />
                <Form.Control.Feedback type="invalid">{errors.name}</Form.Control.Feedback>
            </Form.Group>

            <Form.Group className="mb-3">
                <Form.Label>From Address</Form.Label>
                <Form.Control
                    name="fromAddress"
                    value={formData.fromAddress}
                    onChange={handleChange}
                    isInvalid={!!errors.fromAddress}
                />
                <Form.Control.Feedback type="invalid">{errors.fromAddress}</Form.Control.Feedback>
            </Form.Group>

            <Form.Group className="mb-3">
                <Form.Label>To Email Address</Form.Label>
                <Form.Control
                    name="toEmailAddress"
                    value={formData.toEmailAddress}
                    onChange={handleChange}
                    isInvalid={!!errors.toEmailAddress}
                />
                <Form.Control.Feedback type="invalid">{errors.toEmailAddress}</Form.Control.Feedback>
            </Form.Group>

            <Form.Group className="mb-3">
                <Form.Label>From Name</Form.Label>
                <Form.Control
                    name="fromName"
                    value={formData.fromName}
                    onChange={handleChange}
                    isInvalid={!!errors.fromName}
                />
                <Form.Control.Feedback type="invalid">{errors.fromName}</Form.Control.Feedback>
            </Form.Group>

            <Button variant="primary" type="submit">Create</Button>
        </Form>
    );
};

export default CreateActivity;
