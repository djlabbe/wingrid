import { Button, Carousel } from 'flowbite-react';
import { HiOutlineArrowLeft, HiOutlineArrowRight } from 'react-icons/hi';
import { get } from '../../services/api';

const AdminOld = () => {
    const handleSelectWeek = async (week: number) => {
        try {
            await get(`/api/events/?season=2023&week=${week}`)
        } catch (e) {
            console.error(e);
        }
    }

    return (
        <>
            <div className="flex justify-center">
                <div className="h-32 w-1/2">
                    <Carousel slide={false} indicators={false} leftControl={
                        <HiOutlineArrowLeft className="h-6 w-6" />
                    } rightControl={
                        <HiOutlineArrowRight className="h-6 w-6" />
                    }>
                        <div className="flex h-full items-center justify-center dark:text-white">
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(0)}>Week 0</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(1)}>Week 1</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(2)}>Week 2</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(3)}>Week 3</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(4)}>Week 4</Button>
                        </div>
                        <div className="flex h-full items-center justify-center dark:text-white">
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(5)}>Week 5</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(6)}>Week 6</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(7)}>Week 7</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(8)}>Week 8</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(9)}>Week 9</Button>
                        </div>
                        <div className="flex h-full items-center justify-center dark:text-white">
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(10)}>Week 10</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(11)}>Week 11</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(12)}>Week 12</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(13)}>Week 13</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(14)}>Week 14</Button>
                        </div>
                        <div className="flex h-full items-center justify-center dark:text-white">
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(15)}>Week 15</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(16)}>Week 16</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(17)}>Week 17</Button>
                            <Button color="dark" className="mx-3" onClick={() => handleSelectWeek(18)}>Week 18</Button>
                        </div>
                    </Carousel>
                </div>
            </div>
            <div>

            </div>
            </>
        )
}

export default AdminOld;