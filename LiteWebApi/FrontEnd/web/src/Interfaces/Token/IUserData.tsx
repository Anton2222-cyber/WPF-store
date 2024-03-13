export default interface IUserData {
    email: string;
    name: string;
    photo: string;
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string[];

    hasRole(role: string): boolean
}