const Hero = () => {
  return (
    <section className="bg-white dark:bg-gray-900">
    <div className="py-8 px-4 mx-auto max-w-screen-xl text-center lg:py-16">
        <h1 className="mb-4 text-4xl font-extrabold tracking-tight leading-none text-gray-900 md:text-5xl lg:text-6xl dark:text-white">Welcome to WINGRID</h1>
        <p className="mb-8 text-lg font-normal text-gray-500 lg:text-xl sm:px-16 lg:px-48 dark:text-gray-400">$20+ and massive bragging rights available EVERY WEEK of the NFL season!</p>
        <div className="flex flex-col space-y-4 sm:flex-row sm:justify-center sm:space-y-0">
            <a href="/signup" className="inline-flex justify-center items-center py-3 px-5 text-base font-medium text-center text-white rounded-lg bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-blue-300 dark:focus:ring-blue-900">
                Sign Up
                <svg className="w-3.5 h-3.5 ms-2 rtl:rotate-180" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 10">
                    <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M1 5h12m0 0L9 1m4 4L9 9"/>
                </svg>
            </a>
            <a href="/about" className="inline-flex justify-center items-center py-3 px-5 sm:ms-4 text-base font-medium text-center text-gray-900 rounded-lg border border-gray-300 hover:bg-gray-100 focus:ring-4 focus:ring-gray-100 dark:text-white dark:border-gray-700 dark:hover:bg-gray-700 dark:focus:ring-gray-800">
              Learn More
            </a>  
        </div>
        <p className="mt-8 mb-3 text-sm font-normal text-gray-500 lg:text-sm dark:text-gray-400">Already have an account? <a href="/login" className="text-green-700">Log In</a></p>
    </div>
</section>


    // <div className="relative isolate px-6 pt-14 lg:px-8">
    //   <div className=" inset-x-0 -z-10 transform-gpu overflow-hidden blur-3xl" aria-hidden="true">
    //     <div className="relative left-[calc(50%-11rem)] aspect-[1155/678] w-[36.125rem] -translate-x-1/2 rotate-[30deg] bg-gradient-to-tr from-[#29961d] to-[#0a5202] opacity-30 sm:left-[calc(50%-30rem)] sm:w-[72.1875rem]" style={{ "clipPath": "polygon(74.1% 44.1%, 100% 61.6%, 97.5% 26.9%, 85.5% 0.1%, 80.7% 2%, 72.5% 32.5%, 60.2% 62.4%, 52.4% 68.1%, 47.5% 58.3%, 45.2% 34.5%, 27.5% 76.7%, 0.1% 64.9%, 17.9% 100%, 27.6% 76.8%, 76.1% 97.7%, 74.1% 44.1%)" }}></div>
    //   </div>
      
    //   <div className="mx-auto max-w-2xl py-32 sm:py-48 lg:py-56">
    //     <div className="text-center">
    //       <h1 className="text-4xl font-bold tracking-tight text-gray-900 sm:text-6xl">WINGRID</h1>
    //       <p className="mt-6 text-lg leading-8 text-gray-600">Pick NCAA & NFL games every week and compete for prizes!</p>
    //       <div className="mt-10 flex items-center justify-center gap-x-6">
    //         <a href="/signup" className="rounded-md bg-green-600 px-3.5 py-2.5 text-sm font-semibold text-white shadow-sm hover:bg-green-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-green-600">Sign Up</a>
    //         <a href="/login" className="text-sm font-semibold leading-6 text-gray-900">Login <span aria-hidden="true">→</span></a>
    //       </div>
    //     </div>
    //   </div>
    //   <div className="absolute inset-x-0 top-[calc(100%-13rem)] -z-10 transform-gpu overflow-hidden blur-3xl sm:top-[calc(100%-30rem)]" aria-hidden="true">
    //     <div className="relative left-[calc(50%+3rem)] aspect-[1155/678] w-[36.125rem] -translate-x-1/2 bg-gradient-to-tr from-[#29961d] to-[#0a5202] opacity-30 sm:left-[calc(50%+36rem)] sm:w-[72.1875rem]" style={{ "clipPath": "polygon(74.1% 44.1%, 100% 61.6%, 97.5% 26.9%, 85.5% 0.1%, 80.7% 2%, 72.5% 32.5%, 60.2% 62.4%, 52.4% 68.1%, 47.5% 58.3%, 45.2% 34.5%, 27.5% 76.7%, 0.1% 64.9%, 17.9% 100%, 27.6% 76.8%, 76.1% 97.7%, 74.1% 44.1%)" }}></div>
    //   </div>
    // </div>
  )
}

export default Hero;