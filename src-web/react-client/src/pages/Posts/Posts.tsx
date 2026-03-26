import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { Link } from "react-router";

type Post = {
  id: string;
  title: string;
  content: string;
  userId: string;
  authorName: string;
  createdAt: string;
  updatedAt: string;
};

export default function Posts() {
  const {
    data: posts,
    isLoading,
    isError,
    error,
  } = useQuery({
    queryKey: ["posts"],

    queryFn: async (): Promise<Post[]> => {
      const response = await axios.get("/api/v1/posts", {
        withCredentials: true,
      });
      return response.data;
    },
  });

  if (isLoading) return <div>Loading...</div>;

  if (isError) return <div>Error: {error.message}</div>;

  const handleFilterSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
  };

  return (
    <section className="px-4 sm:px-10 flex flex-col gap-10">
      {/* title and button  */}
      <div className="flex gap-10 items-center">
        <h1 className="font-bold text-3xl">Posts</h1>

        <nav className="">
          <Link to="/posts/create">
            <button className="bg-gray-800 text-white py-2 px-4 rounded-sm cursor-pointer text-sm">
              Create new
            </button>
          </Link>
        </nav>
      </div>

      {/* filter form  */}
      <form
        onSubmit={handleFilterSubmit}
        className="flex flex-col lg:flex-row gap-2 text-sm w-full"
      >
        <input
          // asp-for="SearchTerm"
          placeholder="Search posts..."
          className="border border-gray-300 rounded-sm p-2 flex-1"
        />

        <select
          // asp-for="OrderBy"
          // asp-items="Html.GetEnumSelectList<PostOrderBy>()"
          className="border border-gray-300 rounded-sm p-2"
          // onchange="this.form.submit()"
        ></select>

        <select
          // asp-for="SortDirection"
          // asp-items="Html.GetEnumSelectList<SortDirection>()"
          className="border border-gray-300 rounded-sm p-2"
          // onchange="this.form.submit()"
        ></select>

        <select
          // asp-for="PageSize"
          className="border border-gray-300 rounded-sm p-2"
          // onchange="this.form.submit()"
        >
          <option value="10">10/page</option>
          <option value="20">20/page</option>
          <option value="30">30/page</option>
        </select>

        <div>
          <input asp-for="Page" value="1" type="hidden" />

          <input asp-for="PageSize" type="hidden" />

          <button
            type="submit"
            className="bg-gray-800 text-white py-2 px-4 rounded-sm cursor-pointer w-fit"
          >
            Filter
          </button>
        </div>
      </form>
      {/* list of posts  */}
      <ul className="mt-10 flex flex-col gap-10 pb-10">
        {posts?.map((post) => {
          return (
            <li key={post.id} className="">
              <div className="flex flex-col lg:flex-row gap-2 items-start justify-between">
                <div className="flex flex-col">
                  <div className="flex items-end gap-4">
                    <Link to={`/posts/${post.id}`} className="flex flex-col">
                      <p className="text-gray-700 font-bold">{post.title}</p>
                    </Link>
                    <Link to={`/users/${post.userId}`} className="">
                      <p className="text-sm font-bold text-gray-500 ">
                        - {post.authorName}
                      </p>
                    </Link>
                  </div>
                  <Link to={`/posts/${post.id}`} className="flex flex-col">
                    <p className="">{post.content}</p>
                  </Link>
                </div>
              </div>
            </li>
          );
        })}
      </ul>
    </section>
  );
}
