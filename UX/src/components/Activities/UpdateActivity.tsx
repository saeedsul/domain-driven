import React from 'react';
import { Form, Button } from 'react-bootstrap';
import { UpdateActivityI, update } from '../../api/activity/activityApi';
import { handleRequest } from '../../utils/requestWrapper';
import DateTimePicker from '../DateTimePicker';
import domEventBus from '../../utils/domEventBus';

interface Props {
    activity?: UpdateActivityI;
    onUpdated: () => void;
}

const UpdateActivity: React.FC<Props> = ({ activity, onUpdated }) => {
    const [formData, setFormData] = React.useState<UpdateActivityI>(() => ({
        id: activity?.id || "",
        name: activity?.name || "",
        fromAddress: activity?.fromAddress || "",
        toEmailAddress: activity?.toEmailAddress || "",
        fromName: activity?.fromName || "",
        openedDate: activity?.openedDate || null,
        bouncedDate: activity?.bouncedDate || null,
    }));

    const [errors, setErrors] = React.useState<Record<string, string>>({});

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value,
        }));

        if (errors[name]) {
            setErrors(prev => ({ ...prev, [name]: '' }));
        }
    };

    const handleDateChange = (name: string, value: string | null) => {
        setFormData(prev => ({
            ...prev,
            [name]: value,
        }));
    };

    const validate = () => {
        const requiredFields = ['name', 'fromAddress', 'toEmailAddress', 'fromName'];
        const newErrors: Record<string, string> = {};

        requiredFields.forEach(field => {
            if (!formData[field as keyof UpdateActivityI]?.toString().trim()) {
                newErrors[field] = 'Field is required';
            }
        });

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!validate()) return;

        const [response, error] = await handleRequest({ promise: update(formData.id, formData) });

        if (error) {
            console.error('Update activity failed:', error);
        } else {
            console.log('Activity updated:', response);
        }

        onUpdated();
        domEventBus.dispatchEvent(new CustomEvent("activityUpdated", { detail: {} }));
    };

    return (
        <Form onSubmit={handleSubmit}>
            <Form.Control type="hidden" name="id" value={formData.id} readOnly />

            <Form.Group className="mb-3">
                <Form.Label>Name</Form.Label>
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

            <DateTimePicker
                label="Opened Date"
                name="openedDate"
                value={formData.openedDate}
                onChange={handleDateChange}
            />

            <DateTimePicker
                label="Bounced Date"
                name="bouncedDate"
                value={formData.bouncedDate}
                onChange={handleDateChange}
            />

            <Button variant="primary" type="submit">
                Update
            </Button>
        </Form>
    );
};

export default UpdateActivity;
