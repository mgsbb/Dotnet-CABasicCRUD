import { useQuery } from "@tanstack/react-query";
import axios from "axios";

type User = {
  id: string;
  name: string;
  email: string;
  username: string;
  createdAt: string;
  updatedAt: string;
  bio: string;
  profileImageUrl: string;
};

export function useAuth() {
  return useQuery({
    queryKey: ["auth"],
    queryFn: async (): Promise<User> => {
      const response = await axios.get("/api/v1/auth/me", {
        withCredentials: true,
      });
      return response.data;
    },
    retry: false,
  });
}
