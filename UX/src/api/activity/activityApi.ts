import axios from "../axiosInstance";

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

export const getAllActivities = async (): Promise<Activity[]> => {
    const response = await axios.get("/Activity/get-all-activities");
    return response.data;
};

export const createActivity = async (activity: Omit<Activity, "id">): Promise<Activity> => {
    const response = await axios.post("/Activity/create-activity", activity);
    return response.data;
};

export const updateActivity = async (id: number, activity: Activity): Promise<Activity> => {
    const response = await axios.put(`/Activity/UpdateActivity/${id}`, activity);
    return response.data;
};

export const deleteActivity = async (id: number): Promise<void> => {
    await axios.delete(`/Activity/DeleteActivity/${id}`);
};
