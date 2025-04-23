import axios from "../../utils/axiosInstance";

export interface Activity {
    id: string;
    name: string;
    fromAddress: string;
    toEmailAddress: string;
    fromName: string;
    createdDate: string;
    bouncedDate?: string | null;
    openedDate?: string | null;
    sentDate: string;
}

export interface AddActivityI { 
    name: string;
    fromAddress: string;
    toEmailAddress: string;
    fromName: string; 
}
export interface UpdateActivityI {
    id: string;
    name: string;
    fromAddress: string;
    toEmailAddress: string;
    fromName: string;
    openedDate?: string | null;
    bouncedDate?: string | null;
}


export const getAllActivities = async (): Promise<Activity[]> => {
    const response = await axios.get("/Activity/get-all-activities");
    return response.data;
};

export const create = async (activity: AddActivityI): Promise<AddActivityI> => {
    const response = await axios.post("/Activity/create-activity", activity);
    return response.data;
};

export const update = async (id: string, activity: UpdateActivityI): Promise<UpdateActivityI> => {
    const response = await axios.put(`/Activity/${id}`, activity);
    return response.data;
};

export const deleteActivity = async (id: string): Promise<void> => {
    await axios.delete(`/Activity/${id}`);
};
