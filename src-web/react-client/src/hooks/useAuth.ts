import { useQuery } from "@tanstack/react-query";
import axios from "axios";

export function useAuth() {
  return useQuery({
    queryKey: ["auth"],
    queryFn: () => {
      return axios.get("/api/v1/auth/me", { withCredentials: true });
    },
    retry: false,
  });
}
